using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    public class MainThreadQueue : MonoBehaviour
    {
        private static MainThreadQueue instance;

        private static readonly Queue<Action<MainThreadQueue>> actionsQueue = new();

        private static int MainThreadId;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                MainThreadId = Thread.CurrentThread.ManagedThreadId;
            }
        }

        private void Update()
        {
            while (actionsQueue.Count > 0)
            {
                var action = actionsQueue.Dequeue();
                try
                {
                    action.Invoke(this);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public static void Enqueue(Action action)
        {
            if (IsOnMainThread())
            {
                action();
            }
            else
            {
                lock (actionsQueue)
                {
                    actionsQueue.Enqueue((_) => action());
                }
            }
        }

        public static void Enqueue(Func<IEnumerator> coroutine)
        {
            if (IsOnMainThread())
            {
                instance.StartCoroutine(coroutine());
            }
            else
            {
                lock (actionsQueue)
                {
                    actionsQueue.Enqueue((mtq) =>
                    {
                        mtq.StartCoroutine(coroutine());
                    });
                }
            }
        }

        private static bool IsOnMainThread() =>
            MainThreadId == Thread.CurrentThread.ManagedThreadId;

        public static void EnqueueAndWait(Func<IEnumerator> coroutine)
        {
            // launch the coroutine in main thread, and wait in this thread for its completion
            using var waitHandle = new ManualResetEvent(false);
            Enqueue(() => EnqueueAndWaitInternalCall(coroutine, waitHandle));
            waitHandle.WaitOne();
        }

        private static IEnumerator EnqueueAndWaitInternalCall(Func<IEnumerator> coroutine, ManualResetEvent waitHandle)
        {
            try
            {
                // make a "yield*" of the enumerator
                var enumerator = coroutine();
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
            finally
            {
                // when it is completed, release the thread
                waitHandle.Set();
            }
        }
    }
}
