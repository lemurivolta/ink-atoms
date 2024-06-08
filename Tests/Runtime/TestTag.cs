using System.Collections;
using System.Collections.Generic;
using LemuRivolta.InkAtoms;
using NUnit.Framework;
using Tests.Runtime.TestTagAssets;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class TestTag
    {
        private bool _afterWait;
        private List<object> _args;

        private bool _beforeWait;
        private StringEvent _continueEvent;

        private int _count = -1;
        private InkAtomsStory _inkAtomsStory;
        private TextAsset _jsonFile;
        private StoryStepVariable _stepAtom;
        private StoryStep _storyStep;
        private TagCoroutine _tagCoroutine;

        private TagWithArgs _tagWithArgs;
        private TagWithoutArgs _tagWithoutArgs;

        private bool _tagWithoutArgsCalled;

        [SetUp]
        public void SetUp()
        {
            // obtains reference to all assets
            _inkAtomsStory = AssetDatabase.LoadAssetAtPath<InkAtomsStory>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestTagAssets/Ink Atoms Story.asset");
            _jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestTagAssets/main.json");
            _stepAtom = AssetDatabase.LoadAssetAtPath<StoryStepVariable>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestTagAssets/Ink Atoms Story - Story Step Variable.asset");
            _continueEvent = AssetDatabase.LoadAssetAtPath<StringEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestTagAssets/Ink Atoms Story - Continue Event.asset");
            // register to tags
            _tagWithoutArgs = AssetDatabase.LoadAssetAtPath<TagWithoutArgs>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestTagAssets/TagWithoutArgs.asset");
            _tagWithoutArgs.Called += TagWithoutArgsOnCalled;
            _tagWithArgs = AssetDatabase.LoadAssetAtPath<TagWithArgs>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestTagAssets/TagWithArgs.asset");
            _tagWithArgs.CountEvent += TagWithArgsOnCountEvent;
            _tagWithArgs.ArgsEvent += TagWithArgsOnArgsEvent;
            _tagCoroutine = AssetDatabase.LoadAssetAtPath<TagCoroutine>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestTagAssets/TagCoroutine.asset");
            _tagCoroutine.StartWait += TagCoroutineOnStartWait;
            _tagCoroutine.EndWait += TagCoroutineOnEndWait;
            // register the story step event
            _stepAtom.GetEvent<StoryStepEvent>().Register(EventFunction);
            // run the story
            _inkAtomsStory.StartStory(_jsonFile);
        }

        private void TagCoroutineOnEndWait()
        {
            Assert.IsFalse(_afterWait);
            _afterWait = true;
        }

        private void TagCoroutineOnStartWait()
        {
            Assert.IsFalse(_beforeWait);
            _beforeWait = true;
        }

        private void TagWithArgsOnArgsEvent(List<object> args)
        {
            Assert.IsNull(_args);
            _args = args;
        }

        private void TagWithArgsOnCountEvent(int count)
        {
            Assert.IsTrue(count >= 0);
            Assert.IsTrue(_count < 0);
            _count = count;
        }

        private void TagWithoutArgsOnCalled()
        {
            Assert.IsFalse(_tagWithoutArgsCalled);
            _tagWithoutArgsCalled = true;
        }

        private void EventFunction(StoryStep storyStep)
        {
            _storyStep = storyStep;
        }

        [UnityTest]
        public IEnumerator Test()
        {
            Assert.IsFalse(_tagWithoutArgsCalled);
            Assert.IsNull(_storyStep);
            _continueEvent.Raise(null);
            Assert.AreEqual("Line 1.", _storyStep?.Text?.Trim());
            Assert.IsTrue(_tagWithoutArgsCalled);

            Assert.IsTrue(_count < 0);
            Assert.IsNull(_args);
            _continueEvent.Raise(null);
            Assert.AreEqual("Line 2.", _storyStep?.Text?.Trim());
            Assert.AreEqual(2, _count);
            Assert.AreEqual(_args, new List<object>(new[]
            {
                "3",
                "hello"
            }));

            Assert.IsFalse(_beforeWait);
            Assert.IsFalse(_afterWait);
            _continueEvent.Raise(null);
            Assert.IsTrue(_beforeWait);
            Assert.IsFalse(_afterWait);
            const int numLoops = 10;
            for (var i = 0; i < numLoops; i++)
            {
                if (_afterWait) break;
                yield return new WaitForSeconds(1.0f / numLoops);
            }

            Assert.IsTrue(_beforeWait);
            Assert.IsTrue(_afterWait);
            Assert.AreEqual("Line 3.", _storyStep?.Text?.Trim());
        }

        [TearDown]
        public void TearDown()
        {
            // deregister the events
            _tagWithoutArgs.Called -= TagWithoutArgsOnCalled;
            _stepAtom.GetEvent<StoryStepEvent>().Unregister(EventFunction);
        }
    }
}