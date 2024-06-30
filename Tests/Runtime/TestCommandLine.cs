using System.Collections;
using System.Collections.Generic;
using LemuRivolta.InkAtoms;
using NUnit.Framework;
using Tests.Runtime.RestartStory;
using Tests.Runtime.TestCommandLineAssets;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class TestCommandLine
    {
        private InkAtomsStory _inkAtomsStory;
        private TextAsset _jsonFile;
        private StoryStepVariable _stepAtom;
        private StringEvent _continueEvent;

        private StoryStep _storyStep;

        private TestCommandLineAction _action;
        private TestCommandLineCoroutine _coroutine;

        [SetUp]
        public void SetUp()
        {
            // obtains reference to all assets
            (_inkAtomsStory, _jsonFile, _stepAtom, _continueEvent, _) =
                Utils.LoadBaseAssets("TestCommandLineAssets");

            _action = AssetDatabase.LoadAssetAtPath<TestCommandLineAction>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestCommandLineAssets/TestCommandLineAction.asset");
            _action.Processed += ActionOnProcessed;

            _coroutine = AssetDatabase.LoadAssetAtPath<TestCommandLineCoroutine>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestCommandLineAssets/TestCommandLineCoroutine.asset");
            _coroutine.StartingProcess += CoroutineOnStartingProcess;
            _coroutine.FinishedProcess += CoroutineOnFinishedProcess;

            // register the story step event
            _stepAtom.GetEvent<StoryStepEvent>().Register(EventFunction);
        }

        [TearDown]
        public void TearDown()
        {
            // deregister the events
            _stepAtom.GetEvent<StoryStepEvent>().Unregister(EventFunction);
            _action.Processed -= ActionOnProcessed;
            _coroutine.StartingProcess -= CoroutineOnStartingProcess;
            _coroutine.FinishedProcess -= CoroutineOnFinishedProcess;
        }

        private readonly List<(string, string)> _actionsProcessed = new();

        private void ActionOnProcessed(string arg1, string arg2)
        {
            _actionsProcessed.Add((arg1, arg2));
        }

        private readonly List<(string, string)> _coroutinesFinishedProcessing = new();

        private void CoroutineOnFinishedProcess(string arg1, string arg2)
        {
            _coroutinesFinishedProcessing.Add((arg1, arg2));
        }

        private readonly List<(string, string)> _coroutinesStartingProcessing = new();

        private void CoroutineOnStartingProcess(string arg1, string arg2)
        {
            _coroutinesStartingProcessing.Add((arg1, arg2));
        }

        private void EventFunction(StoryStep storyStep)
        {
            _storyStep = storyStep;
        }

        [UnityTest]
        public IEnumerator Test()
        {
            _inkAtomsStory.StartStory(_jsonFile, exception => Assert.Fail(exception.ToString()));

            Assert.That(_actionsProcessed, Is.Empty);
            Assert.That(_coroutinesStartingProcessing, Is.Empty);
            Assert.That(_coroutinesFinishedProcessing, Is.Empty);

            _continueEvent.Raise(null);

            Assert.That(_actionsProcessed, Is.Empty);
            Assert.That(_coroutinesStartingProcessing, Is.Empty);
            Assert.That(_coroutinesFinishedProcessing, Is.Empty);
            Assert.That(_storyStep.Text.Trim(), Is.EqualTo("First line."));

            _continueEvent.Raise(null);

            Assert.That(_actionsProcessed, Is.EqualTo(new[] { ("value1", "value 2") }));
            Assert.That(_coroutinesStartingProcessing, Is.Empty);
            Assert.That(_coroutinesFinishedProcessing, Is.Empty);
            Assert.That(_storyStep.Text.Trim(), Is.EqualTo("Second line."));
            
            _continueEvent.Raise(null);

            Assert.That(_actionsProcessed, Is.EqualTo(new[] { ("value1", "value 2") }));
            Assert.That(_coroutinesStartingProcessing, Is.EqualTo(new[] { ("value3", "value 4")}));
            Assert.That(_coroutinesFinishedProcessing, Is.Empty);
            Assert.That(_storyStep.Text.Trim(), Is.EqualTo("Second line."));

            const int numSteps = 100;
            const float maxWaitTime = 1f;
            for (var i = 0; i < numSteps; i++)
            {
                if (_coroutinesFinishedProcessing.Count == 0)
                {
                    yield return new WaitForSeconds(maxWaitTime / numSteps);
                }
            }

            Assert.That(_actionsProcessed, Is.EqualTo(new[] { ("value1", "value 2") }));
            Assert.That(_coroutinesStartingProcessing, Is.EqualTo(new[] { ("value3", "value 4")}));
            Assert.That(_coroutinesFinishedProcessing, Is.EqualTo(new[] { ("value3", "value 4")}));
            Assert.That(_storyStep.Text.Trim(), Is.EqualTo("Third line."));

            // TODO: choices after command
        }
    }
}