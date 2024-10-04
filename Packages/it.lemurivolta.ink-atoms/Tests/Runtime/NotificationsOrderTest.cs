using System.Collections.Generic;
using LemuRivolta.InkAtoms;
using NUnit.Framework;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;

namespace Tests.Runtime
{
    public class NotificationsOrderTest
    {
        private InkAtomsStory _inkAtomsStory;
        private TextAsset _jsonFile;
        private StoryStepVariable _stepAtom;
        private StringEvent _continueEvent;

        private List<int> _callEvents = new();
        private IntEvent _xVariableChangedIntEvent;
        private VoidEvent _onCommand;

        [SetUp]
        public void SetUp()
        {
            // obtains reference to all assets
            (_inkAtomsStory, _jsonFile, _stepAtom, _continueEvent, _) = Utils.LoadBaseAssets("TestNotificationsOrder");
            // load other assets
            _xVariableChangedIntEvent = AssetDatabase.LoadAssetAtPath<IntEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestNotificationsOrder/XVariableChangedIntEvent.asset");
            Assert.IsNotNull(_xVariableChangedIntEvent);
            _onCommand = AssetDatabase.LoadAssetAtPath<VoidEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestNotificationsOrder/OnCommand.asset");
            Assert.IsNotNull(_onCommand);
            // hook up events
            _xVariableChangedIntEvent.Register(OnXVariableChanged);
            _onCommand.Register(OnCommand);
        }

        private void OnCommand(Void obj)
        {
            _callEvents.Add(1);
        }

        private void OnXVariableChanged(int obj)
        {
            _callEvents.Add(2);
        }

        private void Error(System.Exception e)
        {
            Assert.Fail(e.ToString());
        }

        [Test]
        public void NotificationsOrder()
        {
            // initialize the story
            _inkAtomsStory.StartStory(_jsonFile, Error);

            // ask the story to continue
            _continueEvent.Raise(null);

            // no calls made yet
            Assert.That(_callEvents.Count, Is.EqualTo(0));

            // continue again, causing the variable change and command to be executed
            _continueEvent.Raise(null);

            // check that calls are made in the right order
            Assert.That(_callEvents, Is.EqualTo(new List<int> { 2, 1 }));
        }

        [TearDown]
        public void TearDown()
        {
            // unregister events
            _xVariableChangedIntEvent.Unregister(OnXVariableChanged);
            _onCommand.Unregister(OnCommand);
            // clear up call events
            _callEvents.Clear();
        }
    }
}