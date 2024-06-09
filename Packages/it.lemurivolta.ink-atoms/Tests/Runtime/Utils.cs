using LemuRivolta.InkAtoms;
using NUnit.Framework;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;

namespace Tests.Runtime
{
    public static class Utils
    {
        public static BaseAssets LoadBaseAssets(string pathPart)
        {
            var inkAtomsStory = AssetDatabase.LoadAssetAtPath<InkAtomsStory>(
                $"Packages/it.lemurivolta.ink-atoms/Tests/Runtime/{pathPart}/Ink Atoms Story.asset");
            Assert.IsNotNull(inkAtomsStory);
            var jsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>(
                $"Packages/it.lemurivolta.ink-atoms/Tests/Runtime/{pathPart}/main.json");
            Assert.IsNotNull(jsonFile);
            var storyStepVariable = AssetDatabase.LoadAssetAtPath<StoryStepVariable>(
                $"Packages/it.lemurivolta.ink-atoms/Tests/Runtime/{pathPart}/Ink Atoms Story - Story Step Variable.asset");
            Assert.IsNotNull(storyStepVariable);
            var continueEvent = AssetDatabase.LoadAssetAtPath<StringEvent>(
                $"Packages/it.lemurivolta.ink-atoms/Tests/Runtime/{pathPart}/Ink Atoms Story - Continue Event.asset");
            Assert.IsNotNull(continueEvent);
            var choiceEvent = AssetDatabase.LoadAssetAtPath<ChosenChoiceEvent>(
                $"Packages/it.lemurivolta.ink-atoms/Tests/Runtime/{pathPart}/Ink Atoms Story - Chosen Choice Event.asset");
            return new BaseAssets(inkAtomsStory, jsonFile, storyStepVariable, continueEvent, choiceEvent);
        }

        public readonly struct BaseAssets
        {
            public readonly InkAtomsStory InkAtomsStory;
            public readonly TextAsset JsonFile;
            public readonly StoryStepVariable StoryStepVariable;
            public readonly StringEvent ContinueEvent;
            public readonly ChosenChoiceEvent ChoiceEvent;

            public BaseAssets(InkAtomsStory inkAtomsStory, TextAsset jsonFile, StoryStepVariable storyStepVariable,
                StringEvent continueEvent, ChosenChoiceEvent choiceEvent)
            {
                InkAtomsStory = inkAtomsStory;
                JsonFile = jsonFile;
                StoryStepVariable = storyStepVariable;
                ContinueEvent = continueEvent;
                ChoiceEvent = choiceEvent;
            }

            public void Deconstruct(out InkAtomsStory inkAtomsStory, out TextAsset jsonFile,
                out StoryStepVariable storyStepVariable, out StringEvent continueEvent,
                out ChosenChoiceEvent choiceEvent)
            {
                inkAtomsStory = InkAtomsStory;
                jsonFile = JsonFile;
                storyStepVariable = StoryStepVariable;
                continueEvent = ContinueEvent;
                choiceEvent = ChoiceEvent;
            }
        }
    }
}