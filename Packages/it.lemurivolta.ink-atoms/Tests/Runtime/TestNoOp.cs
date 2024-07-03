using System;
using System.Collections;
using LemuRivolta.InkAtoms;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;
using NUnit.Framework;
using Tests.Runtime.NoOp;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class TestNoOp
    {
        private InkAtomsStory _inkAtomsStory;
        private TextAsset _jsonFile;
        private StoryStepVariable _stepAtom;
        private StringEvent _continueEvent;

        private AsyncFunc _asyncFunc;

        [SetUp]
        public void Setup()
        {
            // obtains reference to all assets
            (_inkAtomsStory, _jsonFile, _stepAtom, _continueEvent, _) =
                Utils.LoadBaseAssets("NoOp");

            _asyncFunc =
                AssetDatabase.LoadAssetAtPath<AsyncFunc>(
                    "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/NoOp/AsyncFunc.asset");
            _numAsyncFuncCalls = 0; 
            _asyncFunc.Value = 1;
            _asyncFunc.Processed += AsyncFuncOnProcessed;
        }

        [TearDown]
        public void TearDown()
        {
            _asyncFunc.Processed -= AsyncFuncOnProcessed;
        }

        private int _numAsyncFuncCalls = 0;

        private void AsyncFuncOnProcessed()
        {
            _numAsyncFuncCalls++;
        }

        private class VariableErrorHandler
        {
            public Action<Exception> Handler;
        }

        [UnityTest]
        public IEnumerator Test()
        {
            VariableErrorHandler v = new()
            {
                Handler = (exception) => Assert.Fail($"Unhandled exception: {exception}")
            };
            _inkAtomsStory.StartStory(_jsonFile, exception => v.Handler(exception));

            // check that no-op do not get executed
            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("First line."));

            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Second line."));

            // check that a no-op between async external functions allow them to be called one after the other
            Assert.That(_numAsyncFuncCalls, Is.EqualTo(0));
            _continueEvent.Raise(null);
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Second line."));
            yield return Utils.WaitUntil(1f, 100, () =>
            {
                if (_numAsyncFuncCalls >= 2)
                {
                    return true;
                }

                Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Second line."));
                return false;
            });
            Assert.That(_numAsyncFuncCalls, Is.EqualTo(2));
            Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Third line: 1 and 2."));

            // check that a missing no-op between async external functions cause an exception
            var errorHandlerCalled = false;
            v.Handler = exception =>
            {
                Assert.That(errorHandlerCalled, Is.False);
                errorHandlerCalled = true;
                Assert.That(exception, Is.TypeOf<UnmanageableAsyncSequenceException>());
            };
            _continueEvent.Raise(null);
            yield return Utils.WaitUntil(1f, 100, () =>
            {
                if (errorHandlerCalled)
                {
                    return true;
                }

                Assert.That(_stepAtom.Value.Text.Trim(), Is.EqualTo("Third line: 1 and 2."));
                return false;
            });
            Assert.That(errorHandlerCalled, Is.True);
        }
    }
}