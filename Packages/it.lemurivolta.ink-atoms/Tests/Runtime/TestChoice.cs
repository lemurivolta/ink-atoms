using System;
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

        private void Error(Exception e)
        {
            Assert.Fail(e.ToString());
        }

        [Test]
        public void Test()
        {
            // initialize the story
            _inkAtomsStory.StartStory(_jsonFile, Error);

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

        [Test]
        public void TestEqualities()
        {
            var chosenChoice1 = new ChosenChoice
            {
                ChoiceIndex = 1,
                FlowName = "flow"
            };
            var chosenChoice2 = new ChosenChoice
            {
                ChoiceIndex = 1,
                FlowName = "flow"
            };
            var chosenChoice3 = new ChosenChoice
            {
                ChoiceIndex = 2,
                FlowName = "flow"
            };
            var chosenChoice4 = new ChosenChoice
            {
                ChoiceIndex = 1,
                FlowName = "flow2"
            };
            var chosenChoice5 = new ChosenChoice
            {
                ChoiceIndex = 5,
                FlowName = "flow2"
            };
            Assert.That(chosenChoice1, Is.EqualTo(chosenChoice2));
            Assert.That(chosenChoice1, Is.Not.EqualTo(chosenChoice3));
            Assert.That(chosenChoice1, Is.Not.EqualTo(chosenChoice4));
            Assert.That(chosenChoice1, Is.Not.EqualTo(chosenChoice5));

            var storyChoice = new StoryChoice
            {
                Index = 1,
                Text = "hi",
                Tags = new[] { "tag1", "tag2" }
            };
            Assert.That(storyChoice, Is.EqualTo(new StoryChoice
            {
                Index = 1,
                Text = "hi",
                Tags = new[] { "tag1", "tag2" }
            }));
            Assert.That(storyChoice, Is.EqualTo(new StoryChoice
            {
                Index = 1,
                Text = "hi",
                Tags = new[] { "tag2", "tag1" }
            }));
            Assert.That(storyChoice, Is.Not.EqualTo(new StoryChoice
            {
                Index = 2,
                Text = "hi",
                Tags = new[] { "tag1", "tag2" }
            }));
            Assert.That(storyChoice, Is.Not.EqualTo(new StoryChoice
            {
                Index = 1,
                Text = "hiii",
                Tags = new[] { "tag1", "tag2" }
            }));
            Assert.That(storyChoice, Is.Not.EqualTo(new StoryChoice
            {
                Index = 1,
                Text = "hi",
                Tags = new[] { "tag1", "tag2", "tag3" }
            }));
            Assert.That(storyChoice, Is.Not.EqualTo(new StoryChoice
            {
                Index = 1,
                Text = "hi",
                Tags = new[] { "tag2" }
            }));
        }

        [TearDown]
        public void TearDown()
        {
            // deregister the story step event
            _stepAtom.GetEvent<StoryStepEvent>().Unregister(EventFunction);
        }
    }
}