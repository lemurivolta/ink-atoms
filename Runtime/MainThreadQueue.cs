using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    ///     A component that sequences actions and coroutines.
    /// </summary>
    public class MainThreadQueue : MonoBehaviour
    {
        /// <summary>
        ///     Maximum number of enumerators processed per loop, in order to avoid locks
        /// </summary>
        private const int MaxActionsPerLoop = 100;

        /// <summary>
        ///     The single instance of this object.
        /// </summary>
        private static MainThreadQueue _instance;

        /// <summary>
        ///     The queue of all the enumerators to run as coroutines.
        /// </summary>
        private static readonly Queue<TaggedEnumerator> EnumeratorsQueue = new();

        /// <summary>
        ///     Whether the queue is currently empty. Useful to immediately run an action in the
        ///     same frame as it starts to execute.
        /// </summary>
        public static bool IsEmpty => EnumeratorsQueue.Count == 0;

        private void Start()
        {
            if (_instance == null)
                // no main thread queue yet: mark this as the thread queue
                // and start background coroutine
                SetAsInstance(this);
            else if (_instance != this)
                // there's already an instance: destroy this one
                Destroy(gameObject);
        }

        private static void SetAsInstance(MainThreadQueue mtq)
        {
            if (_instance != null) throw new InvalidOperationException("There already is an instance");

            _instance = mtq;
            DontDestroyOnLoad(mtq.gameObject);
            mtq.StartCoroutine(mtq.QueueManagerCoroutine());
        }

        /// <summary>
        ///     Stop all the currently running coroutines, empty the queue, and start anew.
        /// </summary>
        public static void ResetQueue()
        {
            _instance.StopAllCoroutines();
            _instance.StartCoroutine(_instance.QueueManagerCoroutine());
        }

        /// <summary>
        ///     Initialize the main thread queue.
        /// </summary>
        /// <returns><c>true</c> if the queue was initialized, <c>false</c> if the queue was already running.</returns>
        public static bool Initialize()
        {
            if (_instance != null) return false;

            var o = new GameObject
            {
                name = "[Ink Atoms Story - Main Thread Queue]"
            };
            var mtq = o.AddComponent<MainThreadQueue>();
            SetAsInstance(mtq);
            return true;
        }

        /// <summary>
        ///     The coroutine that constantly process elements in the queue.
        /// </summary>
        /// <returns></returns>
        private IEnumerator QueueManagerCoroutine()
        {
            // keep looping
            for (;;)
            {
                // wait for an action to run
                while (EnumeratorsQueue.Count == 0) yield return null;

                // execute all the actions we found (up to maxActionsPerLoop)
                var i = 0;
                var didYield = false;
                while (EnumeratorsQueue.Count > 0 && i++ < MaxActionsPerLoop)
                {
                    var (enumeratorName, enumerator) = EnumeratorsQueue.Dequeue();
                    Debug.Log($"MTQ - {enumeratorName}");
                    while (enumerator != null && enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                        didYield = true;
                    }
                }

                if (i >= MaxActionsPerLoop)
                    Debug.LogWarning(
                        $"Played more than {MaxActionsPerLoop} actions this loop: maybe an action enqueues other actions recursively?");

                if (!didYield)
                    // allow at least one frame after this to keep responsive
                    yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        /// <summary>
        ///     Enqueue a synchronous action.
        /// </summary>
        /// <param name="action">The action to enqueue.</param>
        /// <param name="name">Name of the action, for debugging purposes.</param>
        public static void Enqueue(Action action, string name = null)
        {
            // make an enumerator that immediately stops
            IEnumerator Wrapper()
            {
                action();
                yield break;
            }

            Enqueue(Wrapper, name);
        }

        /// <summary>
        ///     Same as <see cref="Enqueue(System.Action, string)" />, but disables the optimizations and
        ///     forces the action to be processed on the next frame.
        /// </summary>
        /// <param name="action">The action to enqueue.</param>
        /// <param name="name"></param>
        public static void EnqueueLater(Action action, string name = null)
        {
            // make an enumerator that immediately stops
            IEnumerator Wrapper()
            {
                action();
                yield break;
            }

            EnqueueLater(Wrapper, name);
        }

        /// <summary>
        ///     Enqueue a coroutine.
        /// </summary>
        /// <param name="iterator">The coroutine.</param>
        /// <param name="name"></param>
        public static void Enqueue(Func<IEnumerator> iterator, string name = null)
        {
            Enqueue(iterator(), name);
        }

        /// <summary>
        ///     Same as <see cref="Enqueue(System.Func{IEnumerator}, string)" />, but disables the optimizations and
        ///     forces the action to be processed on the next frame.
        /// </summary>
        /// <param name="iterator">The coroutine.</param>
        /// <param name="name"></param>
        private static void EnqueueLater(Func<IEnumerator> iterator, string name = null)
        {
            EnqueueLater(iterator(), name);
        }

        /// <summary>
        ///     Enqueue an enumerator.
        /// </summary>
        /// <param name="enumerator">The enumerator returned from the coroutine.</param>
        /// <param name="name"></param>
        private static void Enqueue(IEnumerator enumerator, string name = null)
        {
            if (IsEmpty)
            {
                // if the queue is empty, then, instead of waiting the next frame we can already
                // start to execute the coroutine now
                // immediately make a single MoveNext of the enumerator, so that it executes the code
                // up to the first yield
                // if it's already stopped, there's nothing more to do (the func was actually synchronous)
                // if there's still stuff, enqueue a new iterator that includes the first element we already got
                // and all the other values remaining from the enumerator
                var canContinue = enumerator.MoveNext();
                if (canContinue)
                {
                    // create an enumerator with single element + all the others from enumerator
                    // is there a reliable way to create it without iterators?
                    // something like this, but Concat concatenates enumerables, not enumerators
                    // Enumerable.Concat(new object[] { enumerator.Current }, enumerator);
                    IEnumerator RebuiltEnumerator()
                    {
                        do
                        {
                            yield return enumerator.Current;
                        } while (enumerator.MoveNext());
                    }

                    EnumeratorsQueue.Enqueue(new TaggedEnumerator(name, RebuiltEnumerator()));
                }
            }
            else
            {
                EnumeratorsQueue.Enqueue(new TaggedEnumerator(name, enumerator));
            }
        }

        /// <summary>
        ///     Same as <see cref="Enqueue(IEnumerator, string)" />, but disables the optimizations and
        ///     forces the action to be processed on the next frame.
        /// </summary>
        /// <param name="enumerator">The enumerator returned from the coroutine.</param>
        /// <param name="name"></param>
        private static void EnqueueLater(IEnumerator enumerator, string name = null)
        {
            IEnumerator LaterEnumerator()
            {
                yield return null;
                while (enumerator.MoveNext()) yield return enumerator.Current;
            }

            Enqueue(LaterEnumerator(), name);
        }

        private readonly struct TaggedEnumerator
        {
            public readonly string Name;
            public readonly IEnumerator Enumerator;

            public TaggedEnumerator(string name, IEnumerator enumerator)
            {
                Name = name;
                Enumerator = enumerator;
            }

            public readonly void Deconstruct(out string name, out IEnumerator enumerator)
            {
                name = Name;
                enumerator = Enumerator;
            }
        }
    }
}