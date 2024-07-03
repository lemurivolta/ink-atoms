using System.Collections;
using LemuRivolta.InkAtoms;
using NUnit.Framework;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class TestAtomAwaiter
    {
        [UnityTest]
        public IEnumerator WaitForVoidEvent()
        {
            var voidEvent = AssetDatabase.LoadAssetAtPath<VoidEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/AtomAwaiter/VoidEvent.asset");
            var awaitEvent = voidEvent.Await();
            Assert.That(awaitEvent.keepWaiting, Is.True);
            yield return new WaitForSeconds(0.2f);
            Assert.That(awaitEvent.keepWaiting, Is.True);
            voidEvent.Raise();
            yield return null;
            Assert.That(awaitEvent.keepWaiting, Is.False);
        }

        [UnityTest]
        public IEnumerator WaitForIntEvent()
        {
            var intEvent = AssetDatabase.LoadAssetAtPath<IntEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/AtomAwaiter/IntEvent.asset");
            var awaitEvent = intEvent.Await();
            Assert.That(awaitEvent.keepWaiting, Is.True);
            yield return new WaitForSeconds(0.2f);
            Assert.That(awaitEvent.keepWaiting, Is.True);
            intEvent.Raise(3);
            yield return null;
            Assert.That(awaitEvent.keepWaiting, Is.False);
        }

        [UnityTest]
        public IEnumerator WaitForIntEventWithPredicate()
        {
            var intEvent = AssetDatabase.LoadAssetAtPath<IntEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/AtomAwaiter/IntEvent.asset");
            var awaitEvent = intEvent.Await(value => value > 5);
            Assert.That(awaitEvent.keepWaiting, Is.True);
            yield return new WaitForSeconds(0.2f);
            Assert.That(awaitEvent.keepWaiting, Is.True);
            intEvent.Raise(3);
            yield return null;
            Assert.That(awaitEvent.keepWaiting, Is.True);
            intEvent.Raise(8);
            yield return null;
            Assert.That(awaitEvent.keepWaiting, Is.False);
        }

        [UnityTest]
        public IEnumerator WaitForIntEventWithOnEvent()
        {
            var intEvent = AssetDatabase.LoadAssetAtPath<IntEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/AtomAwaiter/IntEvent.asset");
            var gotEvent = -1;
            var awaitEvent = intEvent.Await(onEvent: value => gotEvent = value);
            Assert.That(awaitEvent.keepWaiting, Is.True);
            Assert.That(gotEvent, Is.EqualTo(-1));
            yield return new WaitForSeconds(0.2f);
            Assert.That(awaitEvent.keepWaiting, Is.True);
            Assert.That(gotEvent, Is.EqualTo(-1));
            intEvent.Raise(3);
            yield return null;
            Assert.That(awaitEvent.keepWaiting, Is.False);
            Assert.That(gotEvent, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator WaitForIntEventWithOnEventAndPredicate()
        {
            var intEvent = AssetDatabase.LoadAssetAtPath<IntEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/AtomAwaiter/IntEvent.asset");
            var gotEvent = -1;
            var awaitEvent = intEvent.Await(
                value => value > 5,
                value => gotEvent = value);
            Assert.That(awaitEvent.keepWaiting, Is.True);
            Assert.That(gotEvent, Is.EqualTo(-1));
            yield return new WaitForSeconds(0.2f);
            Assert.That(awaitEvent.keepWaiting, Is.True);
            Assert.That(gotEvent, Is.EqualTo(-1));
            intEvent.Raise(3);
            yield return null;
            Assert.That(awaitEvent.keepWaiting, Is.True);
            Assert.That(gotEvent, Is.EqualTo(-1));
            intEvent.Raise(8);
            yield return null;
            Assert.That(awaitEvent.keepWaiting, Is.False);
            Assert.That(gotEvent, Is.EqualTo(8));
        }

        [UnityTest]
        public IEnumerator WaitForIntVariable()
        {
            var intVariable = AssetDatabase.LoadAssetAtPath<IntVariable>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/AtomAwaiter/IntVariable.asset");
            intVariable.Value = 0;
            var awaitYield = intVariable.Await(value => value > 3);
            Assert.That(awaitYield.keepWaiting, Is.True);
            yield return new WaitForSeconds(0.2f);
            Assert.That(awaitYield.keepWaiting, Is.True);
            intVariable.Value = 3;
            yield return null;
            Assert.That(awaitYield.keepWaiting, Is.True);
            intVariable.Value = 5;
            yield return null;
            Assert.That(awaitYield.keepWaiting, Is.False);
        }
    }
}