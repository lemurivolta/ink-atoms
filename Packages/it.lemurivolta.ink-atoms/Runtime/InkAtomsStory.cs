using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
    /// <summary>
    ///     The class for the scriptable object that handles the ink atoms story. Almost all the interactions are performed
    ///     through Unity Atoms objects.
    /// </summary>
    public class InkAtomsStory : ScriptableObject
    {
        [SerializeField] [Tooltip("Event raised when a new story step happens")]
        private StoryStepVariable storyStepVariable;

        [SerializeField] [Tooltip("Event listened to in order to know when to continue a flow")]
        private StringEvent continueEvent;

        [SerializeField] [Tooltip("Event listened to in order to know which choice to take")]
        private ChosenChoiceEvent choiceEvent;

        [SerializeField] [Tooltip("Variable where to save this InkAtoms once initialized")]
        private InkAtomsStoryVariable inkAtomsStoryInitializedVariable;

        [SerializeField] [Tooltip("Whether to print the current state on console at each step")]
        private bool debugCurrentState;

        /// <summary>
        ///     String builder used to produce debug infos. Only one is created and re-used at each step,
        ///     if necessary.
        /// </summary>
        private StringBuilder _debugContent;

        /// <summary>
        ///     The main thread queue used to handle asynchronous operations.
        /// </summary>
        private MainThreadQueue _mainThreadQueue;

        /// <summary>
        ///     The compiled ink story in memory.
        /// </summary>
        private Story _story;

        /// <summary>
        ///     An (unsafe) access to the underlying Story object. This object could be in a different
        ///     state than the one of the atom variables and events.
        /// </summary>
        public Story unsafeStory => _story;

        /// <summary>
        ///     A counter that is increased each time the story continues.
        /// </summary>
        private int _storyStepCounter;

        /// <summary>
        ///     The main thread queue used to handle asynchronous operations.
        /// </summary>
        internal MainThreadQueue mainThreadQueue => _mainThreadQueue;

        private void OnDestroy()
        {
            // when this object is destroy, also release the story and all the related data. 
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
            inkFileAsset = setupMainInkFile;
            storyStepVariable = setupStoryStepVariable;
            continueEvent = setupContinueEvent;
            choiceEvent = setupChoiceEvent;
            inkAtomsStoryInitializedVariable = setupInkStoryAtomsInitializedVariable;
        }
#endif

        /// <summary>
        ///     Set up a story. It assumes no story is currently running.
        /// </summary>
        /// <param name="inkTextAsset">The asset to load.</param>
        private void Setup(TextAsset inkTextAsset)
        {
            // checks input and state
            if (!inkTextAsset) throw new ArgumentNullException(nameof(inkTextAsset));

            Assert.IsFalse(string.IsNullOrWhiteSpace(inkTextAsset.text),
                "Ink Text Asset must point to a non-empty ink story");
            Assert.IsNotNull(storyStepVariable);
            Assert.IsNotNull(continueEvent);
            Assert.IsNotNull(choiceEvent);
            Assert.IsNotNull(inkAtomsStoryInitializedVariable);

            // create the main thread queue
            _mainThreadQueue = MainThreadQueue.Create();
            _mainThreadQueue.ExceptionEvent += OnException;

            //storyStepCounter = 0;

            // create an ink story and connects the events
            _story = new Story(inkTextAsset.text);
            _story.onDidContinue += Story_onDidContinue;
            _story.onError += Story_onError;

            // initialize the subsystems: variables, external functions, command line parsers
            SetupVariableStorage();
            SetupExternalFunctions();
            SetupCommandLineParsers();

            // register to the external events asking to continue or take a choice
            continueEvent.Register(ContinueFromEvent);
            choiceEvent.Register(ChooseFromEvent);

            // now that we're initialized, save "this" to the variable in order to notice other components
            // we're ready
            inkAtomsStoryInitializedVariable.Value = this;
        }

        /// <summary>
        ///     Release all the resources.
        /// </summary>
        private void Teardown()
        {
            // immediately advertise that this story is no longer valid
            inkAtomsStoryInitializedVariable.Value = null;

            // check if there's actually something to release.
            if (_story == null) return;

            // unregister from external events
            choiceEvent.Unregister(ChooseFromEvent);
            continueEvent.Unregister(ContinueFromEvent);

            // disable the subsystems that need it
            OnDisableVariableStorage();
            OnDisableExternalFunctions();

            // deregister events and null-reference the story so that it can be garbage collected
            _story.onDidContinue -= Story_onDidContinue;
            _story.onError -= Story_onError;
            _story = null;

            // destroy the current main thread queue
            _mainThreadQueue.Destroy();
        }

        /// <summary>
        ///     Start the story using the given text asset. If a story is already in progress, it
        ///     will be discarded and a new one will be started.
        /// </summary>
        /// <param name="inkTextAsset">The asset to read the story from.</param>
        /// <param name="errorHandler">The handler for the asynchronous errors.</param>
        public void StartStory(TextAsset inkTextAsset, Action<Exception> errorHandler)
        {
            if (inkTextAsset == null) throw new ArgumentNullException(nameof(inkTextAsset));

            _errorHandler = errorHandler ?? throw new ArgumentException(nameof(errorHandler));

            Teardown();
            Setup(inkTextAsset);
        }

        /// <summary>
        ///     Callback function for when the main story object continues.
        /// </summary>
        private void Story_onDidContinue()
        {
            // print debug and get current step
            DebugCurrentState();
            var currentStoryStep = GetCurrentStoryStep();
            _storyStepCounter++;

            // if it's a no-op, there's nothing to do, just continue
            if (IsNoOp(currentStoryStep))
            {
                _mainThreadQueue.Enqueue(() => Continue(_story.currentFlowName), "noop continue");
                return;
            }

            // use the tag processors
            ProcessTags(currentStoryStep);

            // use command processors
            var isCommand = TryProcessCommand(currentStoryStep);

            // only if no command was processed, and we're not inside a Call, then save the step
            if (!isCommand && !_inEvaluateFunction)
                _mainThreadQueue.Enqueue(() => storyStepVariable.Value = currentStoryStep,
                    "setting normal line on currentStoryStep");
        }

        /// <summary>
        ///     Checks if the story is at a "@" no-op line.
        /// </summary>
        /// <param name="currentStoryStep">The current story step.</param>
        /// <returns>Whether this is a no-op instruction.</returns>
        private bool IsNoOp(StoryStep currentStoryStep)
        {
            return currentStoryStep.Text.Trim() == cmdLinePrefix.Trim() && currentStoryStep.CanContinue;
        }

        /// <summary>
        ///     Print debug information about the current state, if the <see cref="debugCurrentState" /> flag
        ///     is on.
        /// </summary>
        private void DebugCurrentState()
        {
            if (!debugCurrentState) return;

            _debugContent ??= new StringBuilder();

            _debugContent.AppendLine("Story current state:");
            _debugContent.AppendLine($"> Flow: {_story.currentFlowName}");
            if (!string.IsNullOrEmpty(_story.currentText))
                _debugContent.AppendLine($"> Text: {_story.currentText.Trim()}");

            if (_story.currentChoices.Count > 0)
            {
                _debugContent.AppendLine("> Choices:");
                foreach (var choice in _story.currentChoices) _debugContent.AppendLine($">   {choice.text}");
            }

            _debugContent.AppendLine($"> Can continue? {_story.canContinue}");
            Debug.Log(_debugContent);
            _debugContent.Clear();
        }

        /// <summary>
        ///     Get the current <see cref="StoryStep" /> info.
        /// </summary>
        /// <returns>The current <see cref="StoryStep" /> info.</returns>
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
        /// <param name="flowName">Name of the flow, or <c>null</c> / empty string to switch to the default flow.</param>
        private void SwitchFlow(string flowName)
        {
            if (!string.IsNullOrEmpty(flowName))
                _story.SwitchFlow(flowName);
            else
                _story.SwitchToDefaultFlow();
        }

        /// <summary>
        ///     The callback called when <see cref="continueEvent" /> is raised. It makes the story continue.
        /// </summary>
        /// <param name="flowName">Name of the flow to continue.</param>
        private void ContinueFromEvent(string flowName)
        {
            Continue(flowName, AddStackTrace("Continue from event"));
        }

        /// <summary>
        ///     If in editor, returns the current stack trace, with given text as a prefix. Otherwise, it just
        ///     returns the text.
        /// </summary>
        /// <param name="text">The text to use as prefix or as the result, according to the mode.</param>
        /// <returns>A stack trace with the given text used as a prefix, or just the string.</returns>
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
                return (text != null ? text + ":\n" : "") + st;
            }
#else
            return text;
#endif
        }

        /// <summary>
        ///     Continue the story to the next line. The action is enqueued.
        /// </summary>
        /// <param name="flowName">Flow where we continue.</param>
        /// <param name="reason">The reason for this continue; if <c>null</c>, it will be "continue direct call"</param>
        private void Continue(string flowName, string reason = null)
        {
            _mainThreadQueue.Enqueue(() =>
            {
                SwitchFlow(flowName);
                _story.Continue();
            }, reason ?? "continue direct call");
        }

        /// <summary>
        ///     Callback function for <see cref="choiceEvent" />. It performed the requested choice.
        /// </summary>
        /// <param name="choice">The choice to perform.</param>
        private void ChooseFromEvent(ChosenChoice choice)
        {
            Choose(choice, AddStackTrace($"Choose {choice.ChoiceIndex} from event"));
        }

        /// <summary>
        ///     Choose a choice in the dialogue and continues.
        /// </summary>
        /// <param name="choice">The choice made.</param>
        /// <param name="reason">The reason for taking this choice; if <c>null</c>, it will be "choose direct call".</param>
        private void Choose(ChosenChoice choice, string reason = null)
        {
            _mainThreadQueue.Enqueue(() =>
            {
                SwitchFlow(choice.FlowName);
                _story.ChooseChoiceIndex(choice.ChoiceIndex);
                _story.Continue();
            }, reason ?? "choose direct call");
        }

        #region error handling

        /// <summary>
        ///     Error handler passed in <see cref="StartStory" />.
        /// </summary>
        private Action<Exception> _errorHandler;

        /// <summary>
        ///     Generic handler for any kind of asynchronous error.
        /// </summary>
        /// <param name="exception">The exception containing the error.</param>
        private void OnException(Exception exception)
        {
            Assert.IsNotNull(_errorHandler);
            _errorHandler(exception);
        }

        /// <summary>
        ///     Handler for errors coming from ink.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="type">The level of error.</param>
        /// <exception cref="Exception">If the error type is too high.</exception>
        private void Story_onError(string message, ErrorType type)
        {
            OnException(new InkEngineException(type, message, _story));
        }

        #endregion

        #region variable observers

        /// <summary>
        ///     A storage with all the variables and their current value. This is used to produce the "pair"
        ///     events, since Ink doesn't inform us of the previous value of the variables.
        /// </summary>
        private Dictionary<string, Value> _variableValues;

        [SerializeField] [SerializeReference] private VariableObserver.VariableObserver[] variableObservers;

        /// <summary>
        ///     Initializes the data needed to handle variables.
        /// </summary>
        private void SetupVariableStorage()
        {
            _variableValues = new Dictionary<string, Value>();
            _story.variablesState.variableChangedEvent += VariablesState_variableChangedEvent;

            foreach (var variableObserver in variableObservers) variableObserver.OnEnable(_story.variablesState);

            // fills _variableValues and raise events with the initial values of the variables
            var variablesState = _story.variablesState;
            foreach (var variableName in variablesState)
            {
                var newValueObj = variablesState.GetVariableWithName(variableName);
                VariablesState_variableChangedEvent(variableName, newValueObj);
            }
        }

        /// <summary>
        ///     De-initialize the data needed to handle variables.
        /// </summary>
        private void OnDisableVariableStorage()
        {
            _variableValues = null; // not strictly necessary, but helps GC
            _variableValues = null;
            _story.variablesState.variableChangedEvent -= VariablesState_variableChangedEvent;
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
                var oldValue2 = _variableValues.GetValueOrDefault(variableName);
                _variableValues[variableName] = newValue;

                // the loop is _inside_ the enqueue, so we make just one coroutine step instead of N
                _mainThreadQueue.Enqueue(() =>
                    {
                        foreach (var variableObserver in variableObservers)
                            variableObserver.ProcessVariableValue(variableName, oldValue2, newValue);
                    }, $"variable {variableName} changed");
            }
            else
            {
                Debug.Log($"Exotic variable {variableName} of type {newValueObj.GetType().Name} changed");
            }
        }

        #endregion

        #region external functions

        [SerializeField] [Tooltip("All the external functions that will be used in the story")]
        private BaseExternalFunctionProcessor[] externalFunctions;

        /// <summary>
        ///     Set up the external function handling, by registering the functions.
        /// </summary>
        private void SetupExternalFunctions()
        {
            foreach (var externalFunction in externalFunctions)
            {
                Assert.IsNotNull(externalFunction, "One of the external functions is null");
                externalFunction.Register(this, _story);
            }
        }

        /// <summary>
        ///     De-register all the functions.
        /// </summary>
        private void OnDisableExternalFunctions()
        {
            foreach (var externalFunction in externalFunctions) externalFunction.Unregister(_story);
        }

        #endregion

        #region command line parsers

        [SerializeField] [Tooltip("All the command line parsers that will be used in the story")]
        private BaseCommandLineProcessor[] commandLineParsers;

        [FormerlySerializedAs("commandLinePrefix")]
        [SerializeField]
        [Tooltip("The prefix used to mark command line parsers")]
        private string cmdLinePrefix = "@";

#if UNITY_EDITOR
        public string commandLinePrefix => cmdLinePrefix;
#endif

        /// <summary>
        ///     The string describing the regular expression used to match command lines. This regex is missing the prefix.
        /// </summary>
        private const string CommandLineParserBaseRegex =
            @"(?<name>[^\s]+)(?<param>\s+(?<paramName>[a-zA-Z]*):(""(?<paramValue>[^""]*)""|(?<paramValue>[^\s]*)))*";

        /// <summary>
        ///     The actual regular expression used to match command lines; it's a cache, meaning it's <c>null</c>
        ///     until actually needed.
        /// </summary>
        private Regex _commandLineParserRegexCache;

        /// <summary>
        ///     Get the regular expression to match command lines.
        /// </summary>
        /// <returns>The regular expression to match command lines.</returns>
        private Regex GetCommandLineParserRegex()
        {
            _commandLineParserRegexCache ??= new Regex(cmdLinePrefix + CommandLineParserBaseRegex);
            return _commandLineParserRegexCache;
        }

        /// <summary>
        ///     Set up the command line parsers and checks for duplicates.
        /// </summary>
        private void SetupCommandLineParsers()
        {
            Assert.IsNotNull(cmdLinePrefix);
            Assert.IsTrue(cmdLinePrefix.Trim().Length > 0,
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

        /// <summary>
        ///     Try to process a command in the current story step.
        /// </summary>
        /// <param name="currentStoryStep">The story step to parse as a command.</param>
        /// <returns>If the story step contains a command, returns <c>true</c>, otherwise returns <c>false</c>.</returns>
        private bool TryProcessCommand(StoryStep currentStoryStep)
        {
            // if the line doesn't start with the command line prefix, it's not a command
            if (currentStoryStep.Text.IndexOf(cmdLinePrefix, StringComparison.Ordinal) != 0) return false;

            return TryProcessCommand(currentStoryStep.Text.Trim(),
                Debug.LogWarning,
                (commandLineParser, parameters) =>
                {
                    // there's actually a command line: create a "coroutine" to handle it.
                    IEnumerator CommandLineCoroutine()
                    {
                        // call the processor and process its coroutine
                        CommandLineProcessorContext context = new(parameters, currentStoryStep.Choices);
                        var enumerator = commandLineParser.InternalProcess(context);
                        while (enumerator.MoveNext()) yield return enumerator.Current; // "up" yield

                        // either continue or take a choice, according to what the context says after the
                        // execution
                        var flowName = _story.currentFlowName;
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

                    // enqueues the coroutine processing the command
                    _mainThreadQueue.Enqueue(CommandLineCoroutine, $"Executing command {commandLineParser.Name}");
                }
            );
        }

        /// <summary>
        ///     Try to process a command in the current story step. This method gets called only if the line starts with a command
        ///     prefix.
        /// </summary>
        /// <param name="commandLine">The line to process.</param>
        /// <param name="errorAction">The callback to use if there's a syntax error in the command line.</param>
        /// <param name="successAction">
        ///     The callback to use if the line contains a command and the parsing was successful; the
        ///     function will be passed the processor to use and a map between argument names and their values.
        /// </param>
        /// <returns>If the story step contains a command, returns <c>true</c>, otherwise returns <c>false</c>.</returns>
        private bool TryProcessCommand(string commandLine,
            Action<string> errorAction,
            Action<BaseCommandLineProcessor, IDictionary<string, object>> successAction)
        {
            // uses the regex to parse the command name and parameters
            var matchCollection = GetCommandLineParserRegex().Matches(
                commandLine);

            string commandName = null;
            string[] paramNames = null;
            string[] paramValues = null;
            // moves through the various matches and groups to save the relevant data
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

            // tries to extract the correct command line parser
            commandName ??= ""; // the "@" line returns a null command
            var commandLineParser = commandLineParsers.FirstOrDefault(clp =>
                clp.Name == commandName);
            if (!commandLineParser)
            {
                errorAction(
                    $"Could not find command '{commandName}' passed in line '{commandLine}'");
                return false;
            }

            // pairs the parameter names and parameter values
            Assert.IsNotNull(paramNames);
            Assert.IsNotNull(paramValues);
            Assert.AreEqual(paramNames.Length, paramValues.Length);
            var parameters = paramNames
                .Zip(paramValues, (paramName, value) => (paramName, value))
                .ToDictionary(p => p.paramName, p => p.value as object);

            // call the success callback
            successAction(commandLineParser, parameters);

            // the parsing finished successfully
            return true;
        }

        #endregion

        #region tag processors

        [SerializeField] [Tooltip("All the tag processors that will be used in the story")]
        private List<BaseTagProcessor> tagProcessors;

        /// <summary>
        ///     Process all the (command) tags in the current story step, if any.
        /// </summary>
        /// <param name="storyStep">The story step to examine.</param>
        private void ProcessTags(StoryStep storyStep)
        {
            // the function called for every successfully parsed tag, with the processor and the parameters
            void SuccessAction(BaseTagProcessor tagProcessor, IReadOnlyList<string> tagParameters)
            {
                _mainThreadQueue.Enqueue(
                    () => tagProcessor.InternalProcess(new TagProcessorContext(tagParameters, storyStep)),
                    $"Processing tag {tagProcessor.Name}");
            }

            // parses every tag
            foreach (var tag in storyStep.Tags) ProcessTag(tag, Debug.LogError, SuccessAction);
        }

        /// <summary>
        ///     Process a single tag.
        /// </summary>
        /// <param name="tag">The tag to parse.</param>
        /// <param name="errorAction">The callback to call if there's a parsing error.</param>
        /// <param name="successAction">The callback to call if this tag is handled by a tag processor.</param>
        private void ProcessTag(string tag, Action<string> errorAction,
            Action<BaseTagProcessor, IReadOnlyList<string>> successAction)
        {
            var tagParts = tag.Split(":");
            var tagName = tagParts[0];
            var tagProcessor = tagProcessors?.FirstOrDefault(t => t.Name == tagName);
            if (!tagProcessor)
                errorAction($"could not find tag processor for '{tagName}'");
            else
                // use ArraySegment to avoid creating a new array
                successAction(tagProcessor, new ArraySegment<string>(tagParts, 1, tagParts.Length - 1));
        }

        #endregion

        #region syntax check

        // syntax check is an editor-only functionality
#if UNITY_EDITOR

        [FormerlySerializedAs("mainInkFile")]
        [Tooltip(
            "List of .ink files to check for syntax errors. These files are checked only in the editor, and not in the builds.")]
        [SerializeField]
        private DefaultAsset inkFileAsset;

        public DefaultAsset mainInkFile => inkFileAsset;

        /// <summary>
        ///     Check if a tag contains some kind of error.
        /// </summary>
        /// <param name="tag">The tag to check.</param>
        /// <param name="syntaxError">The error, if any.</param>
        /// <returns>Whether the tag contains some error.</returns>
        public bool HasTagErrors(string tag, out string syntaxError)
        {
            var hasErrors = false;
            var error = string.Empty;

            // uses ProcessTag and saves the error if it happens (do nothing if there's no error)
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

            // use TryProcessCommand to catch any error and save it (do nothing if it succeeds)
            TryProcessCommand(text.Trim(), e =>
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

        /// <summary>
        ///     A flag that is set to true during an ink function call. This is needed, otherwise this object will receive Continue
        ///     events and mistake them for events to be propagated.
        /// </summary>
        private bool _inEvaluateFunction;

        /// <summary>
        ///     Call an Ink function.
        /// </summary>
        /// <param name="functionName">The name of the function to call.</param>
        /// <param name="textOutput">The output produced by this function.</param>
        /// <param name="arguments">Arguments passed to the function.</param>
        /// <returns>The return value of the function.</returns>
        public object Call(string functionName, out string textOutput, params object[] arguments)
        {
            object result;
            // mark the flag in entry and exit
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

        /// <summary>
        ///     Get the definition of a list by name.
        /// </summary>
        /// <param name="listName">Name of the list to obtain the definition of.</param>
        /// <param name="listDefinition">
        ///     When the method returns, contains the definition of the list, if it was found; otherwise,
        ///     contains <c>null</c>.
        /// </param>
        /// <returns><c>true</c> if the currently running Ink Story contains the given list name, <c>false</c> otherwise.</returns>
        public bool TryGetListDefinition(string listName, out ListDefinition listDefinition)
        {
            if (_story == null)
            {
                listDefinition = null;
                return false;
            }

            return _story.listDefinitions.TryListGetDefinition(listName, out listDefinition);
        }

        /// <summary>
        ///     Get the definition of a list by name.
        /// </summary>
        /// <param name="listName">Name of the list to obtain the definition of.</param>
        /// <returns>The list definition, or <c>null</c> if it was not found.</returns>
        private ListDefinition GetListDefinition(string listName)
        {
            if (_story == null || !_story.listDefinitions.TryListGetDefinition(listName, out var def))
                return null;
            return def;
        }

        /// <summary>
        ///     Create an ink list from one or more origins.
        /// </summary>
        /// <param name="originNames">The name of the lists that are the origins for the list to create.</param>
        /// <returns>The new ink list</returns>
        public InkList CreateInkListFromListDefinitions(params string[] originNames)
        {
            return new InkList
            {
                origins = originNames.Select(GetListDefinition).ToList()
            };
        }

        #endregion
    }
}