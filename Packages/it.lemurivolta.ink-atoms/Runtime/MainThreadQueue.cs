using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    /// A component that sequences actions and coroutines.
    /// </summary>
    public class MainThreadQueue : MonoBehaviour
    {
        /// <summary>
        /// The single instance of this object.
        /// </summary>
        private static MainThreadQueue instance;

        /// <summary>
        /// The queue of all the enumerators to run as coroutines.
        /// </summary>
        private static readonly Queue<IEnumerator> enumeratorsQueue = new();

        /// <summary>
        /// Maximum number of enumerators processed per loop, in order to avoid locks
        /// </summary>
        private const int maxActionsPerLoop = 100;

        /// <summary>
        /// Whether the queue is currently empty. Useful to immediately run an action in the
        /// same frame as it starts to execute.
        /// </summary>
        public static bool IsEmpty => enumeratorsQueue.Count == 0;

        private void Start()
        {
            if (instance == null)
            {
                // no main thread queue yet: mark this as the thread queue
                // and start background coroutine
                SetAsInstance(this);
            }
            else if (instance != this)
            {
                // there's already an instance: destroy this one
                Destroy(gameObject);
            }
        }

        private static void SetAsInstance(MainThreadQueue mtq)
        {
            if (instance != null)
            {
                throw new System.InvalidOperationException("There already is an instance");
            }
            instance = mtq;
            DontDestroyOnLoad(mtq.gameObject);
            mtq.StartCoroutine(mtq.QueueManagerCoroutine());
        }

        /// <summary>
        /// Initialize the main thread queue.
        /// </summary>
        public static void Initialize()
        {
            if (instance != null)
            {
                return;
            }
            var o = new GameObject
            {
                name = "[Ink Atoms Story - Main Thread Queue]"
            };
            var mtq = o.AddComponent<MainThreadQueue>();
            SetAsInstance(mtq);
        }

        /// <summary>
        /// The coroutine that constantly process elements in the queue.
        /// </summary>
        /// <returns></returns>
        private IEnumerator QueueManagerCoroutine()
        {
            // keep looping
            for (; ; )
            {
                // wait for an action to run
                while (enumeratorsQueue.Count == 0)
                {
                    yield return null;
                }
                // execute all the actions we found (up to maxActionsPerLoop)
                var i = 0;
                while (enumeratorsQueue.Count > 0 && i++ < maxActionsPerLoop)
                {
                    var enumerator = enumeratorsQueue.Dequeue();
                    while (enumerator != null && enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }
                }
                if (i >= maxActionsPerLoop)
                {
                    Debug.LogWarning($"Played more than {maxActionsPerLoop} actions this loop: maybe an action enqueues other actions recursively?");
                }
                // allow at least one frame after this to keep responsive
                yield return null;
            }
        }

        /// <summary>
        /// Enqueue a synchronous action.
        /// </summary>
        /// <param name="action">The action to enqueue.</param>
        public static void Enqueue(System.Action action)
        {
            // make an enumerator that immediately stops
            IEnumerator Wrapper()
            {
                action();
                yield break;
            }
            Enqueue(Wrapper);
        }

        /// <summary>
        /// Enqueue a coroutine.
        /// </summary>
        /// <param name="iterator">The coroutine.</param>
        public static void Enqueue(System.Func<IEnumerator> iterator) => Enqueue(iterator());

        /// <summary>
        /// Enqueue an enumerator.
        /// </summary>
        /// <param name="enumerator">The enumerator returned from the coroutine.</param>
        public static void Enqueue(IEnumerator enumerator)
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
                    enumeratorsQueue.Enqueue(RebuiltEnumerator());
                }
            }
            else
            {
                enumeratorsQueue.Enqueue(enumerator);
            }
        }
    }
}
