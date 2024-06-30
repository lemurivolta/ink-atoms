using System;
using System.Collections;
using System.Collections.Generic;
using LemuRivolta.InkAtoms;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;
using NUnit.Framework;
using Tests.Runtime.RestartStory;
using Tests.Runtime.TestExternalFunctionAssets;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.Runtime
{
    public class TestRestart
    {
        private InkAtomsStory _inkAtomsStory;
        private TextAsset _jsonFile;
        private StoryStepVariable _stepAtom;
        private StringEvent _continueEvent;

        private StoryStep _storyStep;

        private RestartExternalFunction _restartExternalFunction;
        private RestartCommand _restartCommand;
        private RestartTagProcessor _restartTagProcessor;
        private IntVariable _var1;

        [SetUp]
        public void SetUp()
        {
            // obtains reference to all assets
            (_inkAtomsStory, _jsonFile, _stepAtom, _continueEvent, _) =
                Utils.LoadBaseAssets("RestartStory");

            _restartExternalFunction = AssetDatabase.LoadAssetAtPath<RestartExternalFunction>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/RestartStory/RestartExternalFunction.asset");
            _restartExternalFunction.ProcessCalled += RestartExternalFunctionOnProcessCalled;

            _commands.Clear();
            _restartCommand = AssetDatabase.LoadAssetAtPath<RestartCommand>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/RestartStory/RestartCommand.asset");
            _restartCommand.Processed += RestartCommandOnProcessed;

            _tags.Clear();
            _restartTagProcessor = AssetDatabase.LoadAssetAtPath<RestartTagProcessor>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/RestartStory/RestartTagProcessor.asset");
            _restartTagProcessor.Performed += RestartTagProcessorOnPerformed;

            _var1 = AssetDatabase.LoadAssetAtPath<IntVariable>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/RestartStory/Var1IntVariable.asset");

            // register the story step event
            _stepAtom.GetEvent<StoryStepEvent>().Register(EventFunction);
        }

        [TearDown]
        public void TearDown()
        {
            // deregister the events
            _stepAtom.GetEvent<StoryStepEvent>().Unregister(EventFunction);
            _restartExternalFunction.ProcessCalled -= RestartExternalFunctionOnProcessCalled;
            _restartCommand.Processed -= RestartCommandOnProcessed;
            _restartTagProcessor.Performed -= RestartTagProcessorOnPerformed;
        }

        private void EventFunction(StoryStep storyStep)
        {
            _storyStep = storyStep;
        }

        private int _numFuncCalls = 0;

        private void RestartExternalFunctionOnProcessCalled()
        {
            _numFuncCalls++;
        }

        private readonly List<int> _commands = new();

        private void RestartCommandOnProcessed(int obj)
        {
            _commands.Add(obj);
        }

        private readonly List<string> _tags = new();

        private void RestartTagProcessorOnPerformed(string obj)
        {
            _tags.Add(obj);
        }

        [Test]
        public void TestRestartOnce()
        {
            for (var i = 0; i < 2; i++)
            {
                Debug.Log($"loop {i}");
                _inkAtomsStory.StartStory(_jsonFile, exception => Assert.Fail(exception.ToString()));
                Assert.That(_commands, Is.Empty);
                Assert.That(_tags, Is.Empty);
                Assert.That(_numFuncCalls, Is.Zero);

                _continueEvent.Raise(null);

                Assert.That(_storyStep.Text.Trim(), Is.EqualTo("First story."));
                Assert.That(_var1.Value, Is.EqualTo(0));
                Assert.That(_commands, Is.Empty);
                Assert.That(_tags, Is.Empty);
                Assert.That(_numFuncCalls, Is.Zero);

                _continueEvent.Raise(null);

                Assert.That(_storyStep.Text.Trim(), Is.EqualTo("Another phrase."));
                Assert.That(_var1.Value, Is.EqualTo(2));
                Assert.That(_commands, Is.EqualTo(new[] { 1 }));
                Assert.That(_tags, Is.EqualTo(new[] { "value" }));
                Assert.That(_numFuncCalls, Is.EqualTo(1));
                
                _commands.Clear();
                _tags.Clear();
                _numFuncCalls = 0;
            }
        }
    }
}