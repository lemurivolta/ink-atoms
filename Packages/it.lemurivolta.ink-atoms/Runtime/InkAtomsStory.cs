using Ink.Runtime;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using UnityAtoms.BaseAtoms;

using UnityEditor;

using UnityEngine;
using UnityEngine.Assertions;

using static LemuRivolta.InkAtoms.CommandLineParser;

namespace LemuRivolta.InkAtoms
{
    public class InkAtomsStory : ScriptableObject
    {
        [Tooltip("Event raised when a new story step happens")]
        [SerializeField] private StoryStepVariable storyStepVariable;

        [Tooltip("Event listened to in order to know when to continue a flow")]
        [SerializeField] private StringEvent continueEvent;

        [Tooltip("Event listened to in order to know which choice to take")]
        [SerializeField] private ChosenChoiceEvent choiceEvent;

        [Tooltip("Event raised when this ink atoms story is initialized")]
        [SerializeField] private InkAtomsStoryEvent inkStoryAtomsInitialized;

        [Tooltip("Whether to print the current state on console at each step")]
        [SerializeField] private bool debugCurrentState;

        /// <summary>
        /// The compiled ink story in memory
        /// </summary>
        private Story story;

#if UNITY_EDITOR
        /// <summary>
        /// An editor-only way to setup the Ink Atoms Story.
        /// </summary>
        /// <param name="storyStepVariable"></param>
        /// <param name="continueEvent"></param>
        /// <param name="choiceEvent"></param>
        public void SetupAsset(
            StoryStepVariable storyStepVariable,
            StringEvent continueEvent,
            ChosenChoiceEvent choiceEvent,
            InkAtomsStoryEvent inkStoryAtomsInitialized)
        {
            this.storyStepVariable = storyStepVariable;
            this.continueEvent = continueEvent;
            this.choiceEvent = choiceEvent;
            this.inkStoryAtomsInitialized = inkStoryAtomsInitialized;
        }
#endif

        private void OnDisable()
        {
            Teardown();
        }

        private void Setup(TextAsset inkTextAsset)
        {
            Assert.IsNotNull(inkTextAsset, "Ink Text Asset must have a value");
            Assert.IsFalse(string.IsNullOrWhiteSpace(inkTextAsset.text),
                "Ink Text Asset must point to a non-empty ink story");
            Assert.IsNotNull(storyStepVariable);
            Assert.IsNotNull(continueEvent);
            Assert.IsNotNull(choiceEvent);
            Assert.IsNotNull(inkStoryAtomsInitialized);

            story = new Story(inkTextAsset.text);
            story.onDidContinue += Story_onDidContinue;

            continueEvent.Register(Continue);
            choiceEvent.Register(Choose);

            OnEnableVariableStorage();
            OnEnableExternalFunctions();
            OnEnableCommandLineParsers();

            inkStoryAtomsInitialized.Raise(this);
        }

        private void Teardown()
        {
            if (story == null)
            {
                return;
            }
            OnDisableVariableStorage();

            choiceEvent.Unregister(Choose);
            continueEvent.Unregister(Continue);

            story.onDidContinue -= Story_onDidContinue;
            story = null;
        }

        /// <summary>
        /// Start the story using the given text asset. If a story is already in progress, its
        /// story will be discarded and a new one will be started.
        /// </summary>
        /// <param name="inkTextAsset">The asset to read the story from.</param>
        public void StartStory(TextAsset inkTextAsset)
        {
            Teardown();
            Setup(inkTextAsset);
        }

        /// <summary>
        /// Callback function for when the main story object continues.
        /// </summary>
        private void Story_onDidContinue()
        {
            DebugCurrentState();
            var currentStoryStep = GetCurrentStoryStep();
            if (IsNoOp(currentStoryStep))
            {
                MainThreadQueue.Enqueue(() => Continue(story.currentFlowName));
                return;
            }
            ProcessTags(currentStoryStep);
            var isCommand = ProcessCommandQueue(currentStoryStep);
            if (isCommand)
            {
                MainThreadQueue.Enqueue(() => Continue(story.currentFlowName));
            }
            else if (!inEvaluateFunction)
            {
                MainThreadQueue.Enqueue(() => storyStepVariable.Value = currentStoryStep);
            }
        }

        /// <summary>
        /// Checks if the story is at a "@" no-op line.
        /// </summary>
        /// <param name="currentStoryStep">The current story step.</param>
        /// <returns>Whether this is a no-op instruction.</returns>
        private bool IsNoOp(StoryStep currentStoryStep) =>
            currentStoryStep.Text.Trim() == "@" && currentStoryStep.CanContinue;

        private void DebugCurrentState()
        {
            if (!debugCurrentState)
            {
                return;
            }

            List<string> debugLines = new();
            void Log(string text)
            {
                debugLines.Add(text);
            }
            Log("Story current state:");
            Log($"> Flow: {story.currentFlowName}");
            if (!string.IsNullOrEmpty(story.currentText))
            {
                Log($"> Text: {story.currentText.Trim()}");
            }
            if (story.currentChoices.Count > 0)
            {
                Log("> Choices:");
                foreach (var choice in story.currentChoices)
                {
                    Log($">   {choice.text}");
                }
            }
            Log($"> Can continue? {story.canContinue}");
            Debug.Log(string.Join('\n', debugLines));
        }

        private StoryStep GetCurrentStoryStep() => new()
        {
            Flow = story.currentFlowName,
            Text = story.currentText,
            Tags = story.currentTags.ToArray(),
            LineNumber = story.debugMetadata == null ? -1 : story.debugMetadata.startLineNumber,
            Choices = story.currentChoices == null || story.currentChoices.Count == 0 ?
                System.Array.Empty<StoryChoice>() :
                (from choice in story.currentChoices
                 select new StoryChoice
                 {
                     Index = choice.index,
                     Text = choice.text,
                     Tags = choice.tags?.ToArray(),
                 }).ToArray(),
            CanContinue = story.canContinue
        };

        /// <summary>
        /// Switch the flow of the current story.
        /// </summary>
        /// <param name="flowName">Name of the flow, or <c>null</c>/empty string to switch to the default flow.</param>
        private void SwitchFlow(string flowName)
        {
            if (!string.IsNullOrEmpty(flowName))
            {
                story.SwitchFlow(flowName);
            }
            else
            {
                story.SwitchToDefaultFlow();
            }
        }

        /// <summary>
        /// Continue the story to the next line. The action is enqueued.
        /// </summary>
        /// <param name="flowName">Flow where we continue.</param>
        private void Continue(string flowName)
        {
            MainThreadQueue.Enqueue(() =>
            {
                SwitchFlow(flowName);
                story.Continue();
            });
        }

        /// <summary>
        /// Choose a choice in the dialogue and continues.
        /// </summary>
        /// <param name="flowName">Flow where we make the choice.</param>
        /// <param name="choiceIndex">Index of the choice that was made.</param>
        private void Choose(ChosenChoice choice) => MainThreadQueue.Enqueue(() =>
        {
            SwitchFlow(choice.FlowName);
            story.ChooseChoiceIndex(choice.ChoiceIndex);
            story.Continue();
        });

        #region variable storage

        private Dictionary<string, object> variableValues;

        [Tooltip("Listeners are called whenever a variable with a certain name is changed")]
        [SerializeField] private VariableListener[] variableListeners;

        /// <summary>
        /// Get the value of a variable.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns>The content of the variable, possibly boxed.</returns>
        public object GetVariableValue(string variableName) => story.variablesState[variableName];

        private void OnEnableVariableStorage()
        {
            variableValues = new();
            story.variablesState.variableChangedEvent += VariablesState_variableChangedEvent;
            LoadVariablesStorage(story.variablesState);
        }

        private void OnDisableVariableStorage()
        {
            story.variablesState.variableChangedEvent -= VariablesState_variableChangedEvent;
        }

        private void LoadVariablesStorage(VariablesState variablesState)
        {
            foreach (var variableName in variablesState)
            {
                var newValueObj = variablesState.GetVariableWithName(variableName);
                VariablesState_variableChangedEvent(variableName, newValueObj);
            }
        }

        /// <summary>
        /// Callback for ink variable changes.
        /// </summary>
        /// <param name="variableName">Name of the variable that changed.</param>
        /// <param name="newValueObj">New value of the variable.</param>
        private void VariablesState_variableChangedEvent(string variableName, Ink.Runtime.Object newValueObj)
        {
            if (newValueObj is Value newValue && newValue.valueType != ValueType.DivertTarget)
            {
                var oldValue = variableValues.ContainsKey(variableName) ? variableValues[variableName] : null;
                var value = newValue.valueObject;
                variableValues[variableName] = newValue.valueObject;

                MainThreadQueue.Enqueue(() =>
                {
                    foreach (var variableListener in variableListeners)
                    {
                        variableListener.ProcessVariableValue(variableName, oldValue, newValue);
                    }
                });
            }
            else
            {
                Debug.Log($"Exotic variable of type {newValueObj.GetType().Name} changed");
            }
        }

        #endregion

        #region external functions

        [Tooltip("All the external functions that will be used in the story")]
        [SerializeField] private BaseExternalFunction[] externalFunctions;

        private void OnEnableExternalFunctions()
        {
            foreach (var externalFunction in externalFunctions)
            {
                externalFunction.Register(story);
            }
        }

        #endregion

        #region command line parsers

        [Tooltip("All the command line parsers that will be used in the story")]
        [SerializeField] private CommandLineParser[] commandLineParsers;

        private void OnEnableCommandLineParsers()
        {
            // check for duplicate commands
            if (commandLineParsers != null)
            {
                var knownCommands = new List<string>();
                foreach (var commandLineParser in commandLineParsers)
                {
                    string name = commandLineParser.CommandName;
                    Assert.IsFalse(knownCommands.Contains(name), $"Duplicate command {name}");
                    knownCommands.Add(name);
                }
            }
        }

        private readonly static Regex commandLineParserRegex = new(
            @"@(?<name>[^ ]*)(?<param> (?<paramName>[a-zA-Z]*):(""(?<paramValue>[^""]*)""|(?<paramValue>[^ ]*)))+");

        private bool ProcessCommandQueue(StoryStep currentStoryStep)
        {
            if (currentStoryStep.Text.IndexOf('@') != 0)
            {
                return false;
            }

            if (!currentStoryStep.CanContinue)
            {
                throw new System.Exception($"Cannot put a command right before a choice (line ${currentStoryStep.LineNumber})");
            }

            return ProcessCommand(currentStoryStep.Text.Trim(),
                Debug.LogWarning,
                (commandLineParser, parameters) =>
                    MainThreadQueue.Enqueue(() => commandLineParser.Invoke(parameters))
            );
        }

        private bool ProcessCommand(string commandLine,
            System.Action<string> errorAction,
            System.Action<CommandLineParser, IDictionary<string, Parameter>> successAction)
        {
            string commandName = null;
            string[] paramNames = null;
            string[] paramValues = null;
            MatchCollection matchCollection = commandLineParserRegex.Matches(
                commandLine);
            foreach (var g in from Match match in matchCollection.Cast<Match>()
                              from Group g in match.Groups
                              select g)
            {
                switch (g.Name)
                {
                    case "name":
                        commandName = g.Value;
                        break;
                    case "paramName":
                        paramNames = g.Captures
                            .Select(capture => capture.Value)
                            .ToArray();
                        break;
                    case "paramValue":
                        paramValues = g.Captures
                            .Select(capture => capture.Value)
                            .ToArray();
                        break;
                }
            }

            commandName ??= ""; // the "@" line returns a null command
            Assert.IsNotNull(commandName);
            var commandLineParser = commandLineParsers.FirstOrDefault(clp =>
                    clp.CommandName == commandName);
            if (commandLineParser == null)
            {
                errorAction(
                    $"Could not find command '{commandName}' passed in line '{commandLine}'");
                return false;
            }

            Assert.IsNotNull(paramNames);
            Assert.IsNotNull(paramValues);
            Assert.AreEqual(paramNames.Length, paramValues.Length);
            var parameters = paramNames.Zip(paramValues, (name, value) =>
                new Parameter()
                {
                    Name = name,
                    Value = value
                });
            successAction(commandLineParser, parameters.ToDictionary(p => p.Name, p => p));
            return true;
        }

        #endregion

        #region tag processors

        [Tooltip("All the tag processors that will be used in the story")]
        [SerializeField] private List<TagProcessor> tagProcessors;

        private void ProcessTags(StoryStep storyStep)
        {
            void SuccessAction(TagProcessor tagProcessor, IReadOnlyList<string> tagParts) =>
                MainThreadQueue.Enqueue(() => tagProcessor.Process(tagParts, storyStep));
            foreach (var tag in storyStep.Tags)
            {
                ProcessTag(tag, Debug.LogError, SuccessAction);
            }
        }

        private void ProcessTag(string tag, System.Action<string> errorAction,
            System.Action<TagProcessor, IReadOnlyList<string>> successAction)
        {
            var tagParts = tag.Split(":");
            var tagName = tagParts[0];
            var tagProcessor = tagProcessors.FirstOrDefault(tag => tag.Name == tagName);
            if (tagProcessor == null)
            {
                errorAction($"could not find tag processor for '{tagName}'");
            }
            else
            {
                successAction(tagProcessor, new System.ArraySegment<string>(tagParts, 1, tagParts.Length - 1));
            }
        }

        #endregion

        #region syntax check
#if UNITY_EDITOR

        [Tooltip("List of .ink files to check for syntax errors. These files are checked only in the editor, and not in the builds.")]
        [SerializeField] private DefaultAsset[] syntaxCheckFiles;

        public DefaultAsset[] SyntaxCheckFiles => syntaxCheckFiles;

        /// <summary>
        /// Check if a tag contains some kind of error.
        /// </summary>
        /// <param name="tag">The tag to check.</param>
        /// <param name="syntaxError">The error, if any.</param>
        /// <returns>Whether the tag contains some error</returns>
        public bool HasTagErrors(string tag, out string syntaxError)
        {
            bool hasErrors = false;
            string error = string.Empty;

            ProcessTag(tag, (e) =>
            {
                hasErrors = true;
                error = e;
            }, (_, _) => { });

            syntaxError = error;

            return hasErrors;
        }

        /// <summary>
        /// Check if a text line contains some kind of error.
        /// </summary>
        /// <param name="tag">The text to check.</param>
        /// <param name="syntaxError">The error, if any.</param>
        /// <returns>Whether the text contains some error</returns>
        public bool HasTextErrors(string text, out string syntaxError)
        {
            string errorString = string.Empty;
            bool hasError = false;
            ProcessCommand(text.Trim(), (e) =>
            {
                hasError = true;
                errorString = e;
            }, (_, _) => { });
            if (hasError)
            {
                syntaxError = errorString;
                return hasError;
            }
            else
            {
                syntaxError = string.Empty;
                return false;
            }
        }

#endif
        #endregion

        #region function call

        private bool inEvaluateFunction;

        public void Call(string functionName, out string textOutput, out object result, params object[] arguments)
        {
            inEvaluateFunction = true;
            try
            {
                result = story.EvaluateFunction(functionName, out textOutput, arguments);
            }
            finally
            {
                inEvaluateFunction = false;
            }
        }
    }

    #endregion
}