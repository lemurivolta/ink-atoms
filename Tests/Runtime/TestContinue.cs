using LemuRivolta.InkAtoms;
using NUnit.Framework;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;

namespace Tests.Runtime
{
    public class TestContinue
    {
        private StringEvent _continueEvent;
        private InkAtomsStory _inkAtomsStory;
        private TextAsset _jsonFile;
        private StoryStepVariable _stepAtom;

        private StoryStep _storyStep;

        [SetUp]
        public void SetUp()
        {
            // obtains reference to all assets
            _inkAtomsStory = AssetDatabase.LoadAssetAtPath<InkAtomsStory>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestContinue/Ink Atoms Story.asset");
            _jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestContinue/main.json");
            _stepAtom = AssetDatabase.LoadAssetAtPath<StoryStepVariable>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestContinue/Ink Atoms Story - Story Step Variable.asset");
            _continueEvent = AssetDatabase.LoadAssetAtPath<StringEvent>(
                "Packages/it.lemurivolta.ink-atoms/Tests/Runtime/TestContinue/Ink Atoms Story - Continue Event.asset");
            // register the story step event
            _stepAtom.GetEvent<StoryStepEvent>().Register(EventFunction);
        }

        [Test]
        public void TestContinueSimplePasses()
        {
            // initialize the story
            _inkAtomsStory.StartStory(_jsonFile);

            // no story step has been produced
            Assert.IsNull(_storyStep);

            // ask the story to continue
            _continueEvent.Raise(null);

            // check that a story step was received in the form we expect
            Assert.IsNotNull(_storyStep);
            Assert.AreEqual("First line.", _storyStep.Text.Trim());
            Assert.IsFalse(_storyStep.CanContinue);
            Assert.IsEmpty(_storyStep.Choices);
            Assert.IsFalse(_storyStep.HasChoices);
            Assert.IsEmpty(_storyStep.Tags);
        }

        private void EventFunction(StoryStep storyStep)
        {
            // check that the event is received only once
            Assert.IsNull(_storyStep);
            _storyStep = storyStep;
        }

        [TearDown]
        public void TearDown()
        {
            // deregister the story step event
            _stepAtom.GetEvent<StoryStepEvent>().Unregister(EventFunction);
        }
    }
}