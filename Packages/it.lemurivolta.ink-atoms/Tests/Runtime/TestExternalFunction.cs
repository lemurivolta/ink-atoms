using System.Collections;
using System.Collections.Generic;
using LemuRivolta.InkAtoms;
using NUnit.Framework;
using Tests.Runtime.TestExternalFunctionAssets;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class TestExternalFunction
    {
        private TestExternalFunctionActionFunction _actionFunction;
        private List<object> _actionFunctionArgs;

        private bool _actionFunctionCalled;
        private TestExternalFunctionActionFunctionWithArgs _actionFunctionWithArgs;
        private StringEvent _continueEvent;
        private TestExternalFunctionCoroutineFunction _coroutineFunction;
        private InkAtomsStory _inkAtomsStory;
        private TextAsset _jsonFile;

        private bool _postWaitCalled;

        private bool _preWaitCalled;
        private StoryStepVariable _stepAtom;

        private StoryStep _storyStep;

        [SetUp]
        public void SetUp()
        {
            // obtains reference to all assets
            (_inkAtomsStory, _jsonFile, _stepAtom, _continueEvent, _) =
                Utils.LoadBaseAssets("TestExternalFunctionAssets");
            _actionFunction = AssetDatabase.LoadAssetAtPath<TestExternalFunctionActionFunction>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestExternalFunctionAssets/TestExternalFunctionActionFunction.asset");
            _actionFunction.Called += ActionFunctionCallback;
            _actionFunctionWithArgs = AssetDatabase.LoadAssetAtPath<TestExternalFunctionActionFunctionWithArgs>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestExternalFunctionAssets/TestExternalFunctionActionFunctionWithArgs.asset");
            _actionFunctionWithArgs.Called += ActionFunctionWithArgsCallback;
            _coroutineFunction = AssetDatabase.LoadAssetAtPath<TestExternalFunctionCoroutineFunction>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestExternalFunctionAssets/TestExternalFunctionCoroutineFunction.asset");
            _coroutineFunction.PreWait += CoroutineFunctionPreWaitCallback;
            _coroutineFunction.PostWait += CoroutineFunctionPostWaitCallback;
            // register the story step event
            _stepAtom.GetEvent<StoryStepEvent>().Register(EventFunction);
        }

        private void CoroutineFunctionPreWaitCallback()
        {
            Assert.IsFalse(_preWaitCalled);
            _preWaitCalled = true;
        }

        private void CoroutineFunctionPostWaitCallback()
        {
            Assert.IsFalse(_postWaitCalled);
            _postWaitCalled = true;
        }

        private void ActionFunctionWithArgsCallback(List<object> obj)
        {
            // check that this function is called only once
            Assert.IsNull(_actionFunctionArgs);
            _actionFunctionArgs = obj;
        }

        private void ActionFunctionCallback()
        {
            // check that this function is called only once
            Assert.IsFalse(_actionFunctionCalled);
            _actionFunctionCalled = true;
        }

        [UnityTest]
        public IEnumerator TestExternalFunctions()
        {
            // initialize the story
            _inkAtomsStory.StartStory(_jsonFile);

            // check that the first function hasn't been called yet
            Assert.IsFalse(_actionFunctionCalled);

            // advance one row: this will call the first function
            _continueEvent.Raise(null);
            Assert.AreEqual("Line 1.", _storyStep.Text.Trim());
            Assert.IsTrue(_actionFunctionCalled);

            // advance one row: this will call the second function (action with args)
            Assert.IsNull(_actionFunctionArgs);
            _continueEvent.Raise(null);
            Assert.AreEqual("Line 2.", _storyStep.Text.Trim());
            Assert.IsNotNull(_actionFunctionArgs);
            Assert.AreEqual(new List<object>(new object[]
            {
                3,
                "hello"
            }), _actionFunctionArgs);

            // advance one row: this will call the third function (function return result)
            _continueEvent.Raise(null);
            Assert.AreEqual("Result funcFunction: 3", _storyStep.Text.Trim());

            // advance one row: this will run the coroutine function
            Assert.IsFalse(_preWaitCalled);
            Assert.IsFalse(_postWaitCalled);
            _continueEvent.Raise(null);
            const string expected = "Result: 4";
            Assert.IsTrue(_preWaitCalled); // code before coroutine immediately gets called
            Assert.IsFalse(_postWaitCalled);
            Assert.AreNotEqual(expected, _storyStep.Text.Trim()); // but line "doesn't" advance
            const int numSteps = 100;
            for (var i = 0; i < numSteps; i++) // wait for the coroutine to terminate, max 1s
            {
                if (_postWaitCalled) break;
                yield return new WaitForSeconds(1f / numSteps);
            }

            Assert.IsTrue(_postWaitCalled); // now the routine must have been called 
            Assert.AreEqual(expected, _storyStep.Text.Trim()); // and line has advanced

            yield return null;
        }

        private void EventFunction(StoryStep storyStep)
        {
            _storyStep = storyStep;
        }

        [TearDown]
        public void TearDown()
        {
            // deregister the events
            _stepAtom.GetEvent<StoryStepEvent>().Unregister(EventFunction);
            _actionFunction.Called -= ActionFunctionCallback;
        }
    }
}