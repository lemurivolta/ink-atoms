using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Ink;
using Ink.Runtime;
using LemuRivolta.InkAtoms.CommandLineProcessors;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;
using LemuRivolta.InkAtoms.TagProcessors;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Object = Ink.Runtime.Object;
using ValueType = Ink.Runtime.ValueType;

namespace LemuRivolta.InkAtoms
{
    public class InkAtomsStory : ScriptableObject
    {
        [Tooltip("Event raised when a new story step happens")] [SerializeField]
        private StoryStepVariable storyStepVariable;

        [Tooltip("Event listened to in order to know when to continue a flow")] [SerializeField]
        private StringEvent continueEvent;

        [Tooltip("Event listened to in order to know which choice to take")] [SerializeField]
        private ChosenChoiceEvent choiceEvent;

        [FormerlySerializedAs("inkStoryAtomsInitializedVariable")]
        [Tooltip("Variable where to save this InkAtoms once initialized")]
        [SerializeField]
        private InkAtomsStoryVariable inkAtomsStoryInitializedVariable;

        [Tooltip("Whether to print the current state on console at each step")] [SerializeField]
        private bool debugCurrentState;

        /// <summary>
        ///     The compiled ink story in memory
        /// </summary>
        private Story _story;

        private int _storyStepCounter;

        private void OnDisable()
        {
            Teardown();
        }

#if UNITY_EDITOR
        /// <summary>
        ///     An editor-only way to set up the Ink Atoms Story.
        /// </summary>
        /// <param name="setupMainInkFile"></param>
        /// <param name="setupStoryStepVariable"></param>
        /// <param name="setupContinueEvent"></param>
        /// <param name="setupChoiceEvent"></param>
        /// <param name="setupInkStoryAtomsInitializedVariable"></param>
        public void SetupAsset(
            DefaultAsset setupMainInkFile,
            StoryStepVariable setupStoryStepVariable,
            StringEvent setupContinueEvent,
            ChosenChoiceEvent setupChoiceEvent,
            InkAtomsStoryVariable setupInkStoryAtomsInitializedVariable)
        {
            mainInkFile = setupMainInkFile;
            storyStepVariable = setupStoryStepVariable;
            continueEvent = setupContinueEvent;
            choiceEvent = setupChoiceEvent;
            inkAtomsStoryInitializedVariable = setupInkStoryAtomsInitializedVariable;
        }
#endif

        private void Setup(TextAsset inkTextAsset)
        {
            Assert.IsNotNull(inkTextAsset, "Ink Text Asset must have a value");
            Assert.IsFalse(string.IsNullOrWhiteSpace(inkTextAsset.text),
                "Ink Text Asset must point to a non-empty ink story");
            Assert.IsNotNull(storyStepVariable);
            Assert.IsNotNull(continueEvent);
            Assert.IsNotNull(choiceEvent);
            Assert.IsNotNull(inkAtomsStoryInitializedVariable);

            if (!MainThreadQueue.Initialize()) MainThreadQueue.ResetQueue();

            //storyStepCounter = 0;

            _story = new Story(inkTextAsset.text);
            _story.onDidContinue += Story_onDidContinue;
            _story.onError += Story_onError;

            OnEnableVariableStorage();
            OnEnableExternalFunctions();
            OnEnableCommandLineParsers();

            continueEvent.Register(ContinueFromEvent);
            choiceEvent.Register(ChooseFromEvent);

            inkAtomsStoryInitializedVariable.Value = this;
        }

        private void Story_onError(string message, ErrorType type)
        {
            var fullMessage = message;
            if (_story != null && _story.debugMetadata != null)
            {
                var d = _story.debugMetadata;
                fullMessage = $"{d.fileName}:{d.startLineNumber}-{d.endLineNumber} {message}";
            }

            switch (type)
            {
                case ErrorType.Error:
                    throw new Exception(fullMessage);
                case ErrorType.Warning:
                    Debug.LogWarning(fullMessage);
                    break;
                case ErrorType.Author:
                    Debug.Log(fullMessage);
                    break;
                default:
                    Debug.LogError($"Unknown error of type {type}");
                    Debug.Log(fullMessage);
                    break;
            }
        }

        private void Teardown()
        {
            if (_story == null) return;

            OnDisableVariableStorage();

            choiceEvent.Unregister(ChooseFromEvent);
            continueEvent.Unregister(ContinueFromEvent);

            _story.onDidContinue -= Story_onDidContinue;
            _story.onError -= Story_onError;
            _story = null;

            inkAtomsStoryInitializedVariable.Value = null;
        }

        /// <summary>
        ///     Start the story using the given text asset. If a story is already in progress, its
        ///     story will be discarded and a new one will be started.
        /// </summary>
        /// <param name="inkTextAsset">The asset to read the story from.</param>
        public void StartStory(TextAsset inkTextAsset)
        {
            Teardown();
            Setup(inkTextAsset);
        }

        /// <summary>
        ///     Callback function for when the main story object continues.
        /// </summary>
        private void Story_onDidContinue()
        {
            DebugCurrentState();
            var currentStoryStep = GetCurrentStoryStep();
            _storyStepCounter++;
            if (IsNoOp(currentStoryStep))
            {
                MainThreadQueue.Enqueue(() => Continue(_story.currentFlowName), "noop continue");
                return;
            }

            ProcessTags(currentStoryStep);
            var isCommand = ProcessCommand(currentStoryStep);
            if (!isCommand && !_inEvaluateFunction)
                MainThreadQueue.Enqueue(() => storyStepVariable.Value = currentStoryStep,
                    "setting normal line on currentStoryStep");
        }

        /// <summary>
        ///     Checks if the story is at a "@" no-op line.
        /// </summary>
        /// <param name="currentStoryStep">The current story step.</param>
        /// <returns>Whether this is a no-op instruction.</returns>
        private bool IsNoOp(StoryStep currentStoryStep)
        {
            return currentStoryStep.Text.Trim() == commandLinePrefix.Trim() && currentStoryStep.CanContinue;
        }

        private void DebugCurrentState()
        {
            if (!debugCurrentState) return;

            List<string> debugLines = new();

            void Log(string text)
            {
                debugLines.Add(text);
            }

            Log("Story current state:");
            Log($"> Flow: {_story.currentFlowName}");
            if (!string.IsNullOrEmpty(_story.currentText)) Log($"> Text: {_story.currentText.Trim()}");

            if (_story.currentChoices.Count > 0)
            {
                Log("> Choices:");
                foreach (var choice in _story.currentChoices) Log($">   {choice.text}");
            }

            Log($"> Can continue? {_story.canContinue}");
            Debug.Log(string.Join('\n', debugLines));
        }

        private StoryStep GetCurrentStoryStep()
        {
            return new StoryStep
            {
                Counter = _storyStepCounter,
                Flow = _story.currentFlowName,
                Text = _story.currentText,
                Tags = _story.currentTags.ToArray(),
                LineNumber = _story.debugMetadata?.startLineNumber ?? -1,
                Choices = _story.currentChoices == null || _story.currentChoices.Count == 0
                    ? Array.Empty<StoryChoice>()
                    : (from choice in _story.currentChoices
                        select new StoryChoice
                        {
                            Index = choice.index,
                            Text = choice.text,
                            Tags = choice.tags == null ? Array.Empty<string>() : choice.tags.ToArray()
                        }).ToArray(),
                CanContinue = _story.canContinue
            };
        }

        /// <summary>
        ///     Switch the flow of the current story.
        /// </summary>
        /// <param name="flowName">Name of the flow, or <c>null</c>/empty string to switch to the default flow.</param>
        private void SwitchFlow(string flowName)
        {
            if (!string.IsNullOrEmpty(flowName))
                _story.SwitchFlow(flowName);
            else
                _story.SwitchToDefaultFlow();
        }

        private void ContinueFromEvent(string flowName)
        {
            var text = "Continue from event";
            text = AddStackTrace(text);
            Continue(flowName, text);
        }

        private static string AddStackTrace(string text)
        {
#if UNITY_EDITOR
            try
            {
                throw new Exception();
            }
            catch (Exception)
            {
                StackTrace st = new(true);
                text = (text != null ? text + ":\n" : "") + st;
            }
#endif
            return text;
        }

        /// <summary>
        ///     Continue the story to the next line. The action is enqueued.
        /// </summary>
        /// <param name="flowName">Flow where we continue.</param>
        /// <param name="reason">The reason for this continue; if <c>null</c>, it will be "continue direct call"</param>
        private void Continue(string flowName, string reason = null)
        {
            MainThreadQueue.Enqueue(() =>
            {
                SwitchFlow(flowName);
                _story.Continue();
            }, reason ?? "continue direct call");
        }

        private void ChooseFromEvent(ChosenChoice choice)
        {
            var text = $"Choose {choice.ChoiceIndex} from event";
            text = AddStackTrace(text);
            Choose(choice, text);
        }

        /// <summary>
        ///     Choose a choice in the dialogue and continues.
        /// </summary>
        /// <param name="choice">The choice made.</param>
        /// <param name="reason">The reason for taking this choice; if <c>null</c>, it will be "choose direct call".</param>
        private void Choose(ChosenChoice choice, string reason = null)
        {
            MainThreadQueue.Enqueue(() =>
            {
                SwitchFlow(choice.FlowName);
                _story.ChooseChoiceIndex(choice.ChoiceIndex);
                _story.Continue();
            }, reason ?? "choose direct call");
        }

        #region variable storage

        private Dictionary<string, object> _variableValues;

        [Tooltip("Listeners are called whenever a variable with a certain name is changed")] [SerializeField]
        private VariableListener[] variableListeners;

        /// <summary>
        ///     Access to the variables of this story.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns>Value of the variable.</returns>
        public object this[string variableName]
        {
            get => _story.variablesState[variableName];
            set => _story.variablesState[variableName] = value;
        }

        private void OnEnableVariableStorage()
        {
            _variableValues = new Dictionary<string, object>();
            _story.variablesState.variableChangedEvent += VariablesState_variableChangedEvent;
            LoadVariablesStorage(_story.variablesState);
        }

        private void OnDisableVariableStorage()
        {
            _story.variablesState.variableChangedEvent -= VariablesState_variableChangedEvent;
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
        ///     Callback for ink variable changes.
        /// </summary>
        /// <param name="variableName">Name of the variable that changed.</param>
        /// <param name="newValueObj">New value of the variable.</param>
        private void VariablesState_variableChangedEvent(string variableName, Object newValueObj)
        {
            if (newValueObj is Value newValue && newValue.valueType != ValueType.DivertTarget)
            {
                var oldValue = _variableValues.GetValueOrDefault(variableName);
                _variableValues[variableName] = newValue.valueObject;

                MainThreadQueue.Enqueue(() =>
                    {
                        foreach (var variableListener in variableListeners)
                            variableListener.ProcessVariableValue(variableName, oldValue, newValue);
                    }, $"variable {variableName} changed");
            }
            else
            {
                Debug.Log($"Exotic variable of type {newValueObj.GetType().Name} changed");
            }
        }

        #endregion

        #region external functions

        [Tooltip("All the external functions that will be used in the story")] [SerializeField]
        private BaseExternalFunctionProcessor[] externalFunctions;

        private void OnEnableExternalFunctions()
        {
            foreach (var externalFunction in externalFunctions) externalFunction.Register(_story);
        }

        #endregion

        #region command line parsers

        [Tooltip("All the command line parsers that will be used in the story")] [SerializeField]
        private CoroutineCommandLineProcessor[] commandLineParsers;

        [Tooltip("The prefix used to mark command line parsers")] [SerializeField]
        private string commandLinePrefix = "@";

#if UNITY_EDITOR
        public string CommandLinePrefix => commandLinePrefix;
#endif

        private const string CommandLineParserBaseRegex =
            @"(?<name>[^\s]+)(?<param>\s+(?<paramName>[a-zA-Z]*):(""(?<paramValue>[^""]*)""|(?<paramValue>[^\s]*)))*";

        private Regex _commandLineParserRegex;

        private Regex GetCommandLineParserRegex()
        {
            _commandLineParserRegex ??= new Regex(commandLinePrefix + CommandLineParserBaseRegex);
            return _commandLineParserRegex;
        }

        private void OnEnableCommandLineParsers()
        {
            Assert.IsNotNull(commandLinePrefix);
            Assert.IsTrue(commandLinePrefix.Trim().Length > 0,
                "Command line parser must contain at least one non-whitespace character");

            // check for duplicate commands
            if (commandLineParsers != null)
            {
                var knownCommands = new List<string>();
                foreach (var commandLineParser in commandLineParsers)
                {
                    var commandName = commandLineParser.Name;
                    Assert.IsFalse(knownCommands.Contains(commandName), $"Duplicate command {commandName}");
                    knownCommands.Add(commandName);
                }
            }
        }

        private bool ProcessCommand(StoryStep currentStoryStep)
        {
            if (currentStoryStep.Text.IndexOf(commandLinePrefix, StringComparison.Ordinal) != 0) return false;

            return ProcessCommand(currentStoryStep.Text.Trim(),
                Debug.LogWarning,
                (commandLineParser, parameters) =>
                {
                    IEnumerator CommandLineCoroutine()
                    {
                        var flowName = _story.currentFlowName;
                        CommandLineProcessorContext context = new(parameters, currentStoryStep.Choices);
                        var enumerator = commandLineParser.Process(context);
                        while (enumerator.MoveNext()) yield return enumerator.Current;

                        if (context.Continue)
                        {
                            if (currentStoryStep.Choices.Length > 0)
                                throw new Exception("Cannot continue a command with choices");
                            Continue(flowName, $"Command {commandLineParser.Name} completed with a continue");
                        }
                        else
                        {
                            if (currentStoryStep.Choices.Length == 0)
                                throw new Exception("Cannot make a choice in a command without choices");
                            Choose(new ChosenChoice
                                {
                                    ChoiceIndex = context.ChoiceIndex,
                                    FlowName = flowName
                                }, $"Command {commandLineParser.Name} completed with choice {context.ChoiceIndex}");
                        }
                    }

                    MainThreadQueue.Enqueue(CommandLineCoroutine, $"Executing command {commandLineParser.Name}");
                }
            );
        }

        private bool ProcessCommand(string commandLine,
            Action<string> errorAction,
            Action<CoroutineCommandLineProcessor, IDictionary<string, object>> successAction)
        {
            string commandName = null;
            string[] paramNames = null;
            string[] paramValues = null;
            var matchCollection = GetCommandLineParserRegex().Matches(
                commandLine);
            foreach (var g in from Match match in matchCollection
                     from Group g in match.Groups
                     select g)
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

            commandName ??= ""; // the "@" line returns a null command
            var commandLineParser = commandLineParsers.FirstOrDefault(clp =>
                clp.Name == commandName);
            if (!commandLineParser)
            {
                errorAction(
                    $"Could not find command '{commandName}' passed in line '{commandLine}'");
                return false;
            }

            Assert.IsNotNull(paramNames);
            Assert.IsNotNull(paramValues);
            Assert.AreEqual(paramNames.Length, paramValues.Length);
            successAction(
                commandLineParser,
                paramNames
                    .Zip(paramValues, (paramName, value) => (paramName, value))
                    .ToDictionary(p => p.paramName, p => p.value as object));
            return true;
        }

        #endregion

        #region tag processors

        [Tooltip("All the tag processors that will be used in the story")] [SerializeField]
        private List<BaseTagProcessor> tagProcessors;

        private void ProcessTags(StoryStep storyStep)
        {
            void SuccessAction(BaseTagProcessor tagProcessor, IReadOnlyList<string> tagParts)
            {
                MainThreadQueue.Enqueue(
                    () => tagProcessor.InternalProcess(new TagProcessorContext(tagParts, storyStep)),
                    $"Processing tag {tagProcessor.Name}");
            }

            foreach (var tag in storyStep.Tags) ProcessTag(tag, Debug.LogError, SuccessAction);
        }

        private void ProcessTag(string tag, Action<string> errorAction,
            Action<BaseTagProcessor, IReadOnlyList<string>> successAction)
        {
            var tagParts = tag.Split(":");
            var tagName = tagParts[0];
            var tagProcessor = tagProcessors?.FirstOrDefault(t => t.Name == tagName);
            if (tagProcessor == null)
                errorAction($"could not find tag processor for '{tagName}'");
            else
                successAction(tagProcessor, new ArraySegment<string>(tagParts, 1, tagParts.Length - 1));
        }

        #endregion

        #region syntax check

#if UNITY_EDITOR

        [Tooltip(
            "List of .ink files to check for syntax errors. These files are checked only in the editor, and not in the builds.")]
        [SerializeField]
        private DefaultAsset mainInkFile;

        public DefaultAsset MainInkFile => mainInkFile;

        /// <summary>
        ///     Check if a tag contains some kind of error.
        /// </summary>
        /// <param name="tag">The tag to check.</param>
        /// <param name="syntaxError">The error, if any.</param>
        /// <returns>Whether the tag contains some error</returns>
        public bool HasTagErrors(string tag, out string syntaxError)
        {
            var hasErrors = false;
            var error = string.Empty;

            ProcessTag(tag, e =>
            {
                hasErrors = true;
                error = e;
            }, (_, _) => { });

            syntaxError = error;

            return hasErrors;
        }

        /// <summary>
        ///     Check if a text line contains some kind of error.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="syntaxError">The error, if any.</param>
        /// <returns>Whether the text contains some error</returns>
        public bool HasTextErrors(string text, out string syntaxError)
        {
            var errorString = string.Empty;
            var hasError = false;
            ProcessCommand(text.Trim(), e =>
            {
                hasError = true;
                errorString = e;
            }, (_, _) => { });
            if (hasError)
            {
                syntaxError = errorString;
                return hasError;
            }

            syntaxError = string.Empty;
            return false;
        }

#endif

        #endregion

        #region function call

        private bool _inEvaluateFunction;

        public object Call(string functionName, out string textOutput, params object[] arguments)
        {
            object result;
            _inEvaluateFunction = true;
            try
            {
                result = _story.EvaluateFunction(functionName, out textOutput, arguments);
            }
            finally
            {
                _inEvaluateFunction = false;
            }

            return result;
        }

        #endregion

        #region list helpers

        public bool TryGetListDefinition(string listName, out ListDefinition def)
        {
            return _story.listDefinitions.TryListGetDefinition(listName, out def);
        }

        private ListDefinition GetListDefinition(string listName)
        {
            if (!_story.listDefinitions.TryListGetDefinition(listName, out var def))
                throw new Exception($"Could not find list definition '{listName}'.");

            return def;
        }

        public InkList GetInkListFromListDefinitions(params string[] listDefinitionNames)
        {
            return new InkList
            {
                origins = listDefinitionNames.Select(GetListDefinition).ToList()
            };
        }

        #endregion
    }
}