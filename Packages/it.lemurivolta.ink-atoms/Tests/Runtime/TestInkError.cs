using LemuRivolta.InkAtoms;
using NUnit.Framework;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Tests.Runtime
{
    public class TestInkError
    {
        private InkAtomsStory _inkAtomsStory;
        private TextAsset _jsonFile;
        private StoryStepVariable _stepAtom;
        private StringEvent _continueEvent;

        [SetUp]
        public void SetUp()
        {
            // obtains reference to all assets
            (_inkAtomsStory, _jsonFile, _stepAtom, _continueEvent, _) =
                Utils.LoadBaseAssets("InkError");
        }

        [Test]
        public void Test()
        {
            var numCalls = 0;
            _inkAtomsStory.StartStory(_jsonFile, exception =>
            {
                numCalls++;
                Assert.That(exception, Is.TypeOf<InkEngineException>());
            });

            _continueEvent.Raise(null);
            Assert.That(numCalls, Is.EqualTo(0));
            _continueEvent.Raise(null);
            Assert.That(numCalls, Is.EqualTo(1));
        }
    }
}