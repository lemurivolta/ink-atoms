using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    ///     A component that sequences actions and coroutines.
    /// </summary>
    internal class MainThreadQueue : MonoBehaviour
    {
        /// <summary>
        ///     Maximum number of enumerators processed per loop, in order to avoid locks
        /// </summary>
        private const int MaxActionsPerLoop = 100;

        /// <summary>
        ///     The queue of all the enumerators to run as coroutines.
        /// </summary>
        private readonly Queue<TaggedEnumerator> _enumeratorsQueue = new();

        /// <summary>
        ///     Whether the queue is currently empty. Useful to immediately run an action in the
        ///     same frame as it starts to execute.
        /// </summary>
        public bool IsEmpty => _enumeratorsQueue.Count == 0;

        /// <summary>
        ///     Event called upon exceptions on the thread queue.
        /// </summary>
        public event Action<Exception> ExceptionEvent;

        /// <summary>
        ///     Stop all the currently running coroutines, empty the queue, and start anew.
        /// </summary>
        private void ResetQueue()
        {
            StopAllCoroutines();
            StartCoroutine(QueueManagerCoroutine());
        }

        /// <summary>
        ///     Initialize a main thread queue.
        /// </summary>
        public static MainThreadQueue Create()
        {
            var o = new GameObject
            {
                name = "[Ink Atoms Story - Main Thread Queue]",
                hideFlags = HideFlags.HideAndDontSave
            };
            var mtq = o.AddComponent<MainThreadQueue>();
            mtq.ResetQueue();
            return mtq;
        }

        public void Destroy()
        {
            Destroy(gameObject);
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
                while (_enumeratorsQueue.Count == 0) yield return null;

                // execute all the actions we found (up to maxActionsPerLoop)
                var i = 0;
                var didYield = false;
                while (_enumeratorsQueue.Count > 0 && i++ < MaxActionsPerLoop)
                {
                    var (enumeratorName, enumerator) = _enumeratorsQueue.Dequeue();
                    Debug.Log($"MTQ - {enumeratorName}");
                    while (MoveNextOrException(enumerator))
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

        private bool MoveNextOrException(IEnumerator enumerator)
        {
            try
            {
                return enumerator != null && enumerator.MoveNext();
            }
            catch (Exception e)
            {
                ExceptionEvent?.Invoke(e);
                return false;
            }
        }

        /// <summary>
        ///     Enqueue a synchronous action.
        /// </summary>
        /// <param name="action">The action to enqueue.</param>
        /// <param name="actionName">Name of the action, for debugging purposes.</param>
        public void Enqueue(Action action, string actionName = null)
        {
            Enqueue(Wrapper, actionName);
            return;

            // make an enumerator that immediately stops
            IEnumerator Wrapper()
            {
                action();
                yield break;
            }
        }

        /// <summary>
        ///     Enqueue a coroutine.
        /// </summary>
        /// <param name="iterator">The coroutine.</param>
        /// <param name="actionName"></param>
        public void Enqueue(Func<IEnumerator> iterator, string actionName = null)
        {
            Enqueue(iterator(), actionName);
        }

        /// <summary>
        ///     Enqueue an enumerator.
        /// </summary>
        /// <param name="enumerator">The enumerator returned from the coroutine.</param>
        /// <param name="actionName"></param>
        private void Enqueue(IEnumerator enumerator, string actionName = null)
        {
            if (IsEmpty)
            {
                // if the queue is empty, then, instead of waiting the next frame we can already
                // start to execute the coroutine now
                // immediately make a single MoveNext of the enumerator, so that it executes the code
                // up to the first yield
                // if it's already stopped, there's nothing more to do (the func was actually synchronous)
                if (!enumerator.MoveNext()) return;

                // if there's still stuff, enqueue a new iterator that includes the first element we already got
                // and all the other values remaining from the enumerator
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

                _enumeratorsQueue.Enqueue(new TaggedEnumerator(actionName, RebuiltEnumerator()));
            }
            else
            {
                _enumeratorsQueue.Enqueue(new TaggedEnumerator(actionName, enumerator));
            }
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

            public void Deconstruct(out string name, out IEnumerator enumerator)
            {
                name = Name;
                enumerator = Enumerator;
            }
        }
    }
}