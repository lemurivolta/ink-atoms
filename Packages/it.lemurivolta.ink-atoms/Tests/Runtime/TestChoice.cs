using LemuRivolta.InkAtoms;
using NUnit.Framework;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Tests.Runtime
{
    public class TestChoice
    {
        private ChosenChoiceEvent _choiceEvent;
        private StringEvent _continueEvent;
        private InkAtomsStory _inkAtomsStory;
        private TextAsset _jsonFile;
        private StoryStepVariable _stepAtom;

        private StoryStep _storyStep;

        [SetUp]
        public void SetUp()
        {
            // obtains reference to all assets
            (_inkAtomsStory, _jsonFile, _stepAtom, _continueEvent, _choiceEvent) =
                Utils.LoadBaseAssets("TestChoiceAssets");
            // register the story step event
            _stepAtom.GetEvent<StoryStepEvent>().Register(EventFunction);
        }

        [Test]
        public void Test()
        {
            // initialize the story
            _inkAtomsStory.StartStory(_jsonFile);

            // no story step has been produced
            Assert.IsNull(_storyStep);

            // ask the story to continue
            _continueEvent.Raise(null);

            // make a choice
            Assert.IsNotNull(_storyStep);
            Assert.IsFalse(_storyStep.CanContinue);
            Assert.AreEqual(2, _storyStep.Choices.Length);
            _choiceEvent.Raise(_storyStep.GetChosenChoice(0));

            // check that a story step was received in the form we expect
            Assert.IsNotNull(_storyStep);
            Assert.AreEqual("First choice.", _storyStep.Text.Trim());
            Assert.IsFalse(_storyStep.CanContinue);
            Assert.IsEmpty(_storyStep.Choices);
            Assert.IsFalse(_storyStep.HasChoices);
            Assert.IsEmpty(_storyStep.Tags);
        }

        private void EventFunction(StoryStep storyStep)
        {
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