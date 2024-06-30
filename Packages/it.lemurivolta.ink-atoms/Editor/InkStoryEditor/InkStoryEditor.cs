using System;
using System.IO;
using System.Text;
using Ink;
using Ink.Parsed;
using LemuRivolta.InkAtoms.Editor.Editor.VariableObservers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using Object = Ink.Parsed.Object;
using Path = System.IO.Path;

namespace LemuRivolta.InkAtoms.Editor.Editor.InkStoryEditor
{
    /// <summary>
    ///     The editor for the type <see cref="InkAtomsStory" />.
    /// </summary>
    [CustomEditor(typeof(InkAtomsStory))]
    public class InkStoryEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        private bool _insideTag;

        public override VisualElement CreateInspectorGUI()
        {
            if (visualTreeAsset == null)
                visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Packages/it.lemurivolta.ink-atoms/Editor/InkStoryEditor/InkStoryEditor.uxml");
            var rootVisualElement = visualTreeAsset.CloneTree();

            var atomsFoldout = rootVisualElement.Q<Foldout>("atoms-foldout");
            atomsFoldout.value = false;

            var syntaxHelpBox = rootVisualElement.Q<HelpBox>("syntax-warnings");
            syntaxHelpBox.style.display = DisplayStyle.None;
            var syntaxCheckButton = rootVisualElement.Q<Button>("syntax-check-button");

            syntaxCheckButton.clicked += () => { CheckSyntax(syntaxHelpBox); };

            var contents = rootVisualElement.Q<VisualElement>("contents");
            var noInkFile = rootVisualElement.Q<HelpBox>("no-ink-file");
            UpdateContentsVisibility(contents, noInkFile);

            rootVisualElement.Q<PropertyField>("main-ink-file").RegisterValueChangeCallback(ev =>
            {
                UpdateContentsVisibility(contents, noInkFile);
            });

            var externalFunctions =
                rootVisualElement.Q<StrategyScriptableObjectListField.StrategyScriptableObjectListField>(
                    "external-functions");
            externalFunctions.Setup(serializedObject);

            var commandLineParsers =
                rootVisualElement.Q<StrategyScriptableObjectListField.StrategyScriptableObjectListField>(
                    "command-line-parsers");
            commandLineParsers.Setup(serializedObject);

            var tagProcessors =
                rootVisualElement.Q<StrategyScriptableObjectListField.StrategyScriptableObjectListField>(
                    "tag-processors");
            tagProcessors.Setup(serializedObject);

            var variableObserverList = rootVisualElement.Q<VariableObserverList>("variable-observer-list");
            variableObserverList.Setup(serializedObject);

            return rootVisualElement;
        }

        private void UpdateContentsVisibility(VisualElement contents, HelpBox noInkFile)
        {
            var enabled = (target as InkAtomsStory).mainInkFile != null;
            contents.SetEnabled(enabled);
            noInkFile.style.display = enabled ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private void CheckSyntax(HelpBox syntaxHelpBox)
        {
            var inkAtomsStory = target as InkAtomsStory;
            StringBuilder sb = new();
            try
            {
                CheckFileSyntax(inkAtomsStory.mainInkFile, sb);
            }
            catch (Exception e)
            {
                sb.AppendLine(e.ToString());
            }

            var content = sb.ToString().Trim();
            if (content.Length != 0)
            {
                syntaxHelpBox.text = content;
                syntaxHelpBox.messageType = HelpBoxMessageType.Warning;
                syntaxHelpBox.style.display = DisplayStyle.Flex;
            }
            else
            {
                syntaxHelpBox.text = "No errors.";
                syntaxHelpBox.messageType = HelpBoxMessageType.Info;
                syntaxHelpBox.style.display = DisplayStyle.Flex;
            }
        }

        private void CheckFileSyntax(DefaultAsset file, StringBuilder sb)
        {
            var parsedStory = ParseStory(file, sb);
            _insideTag = false;
            Visit(parsedStory, sb);
        }

        private void Visit(Object parsedObject, StringBuilder sb)
        {
            // visit recursively
            if (parsedObject.content != null)
                foreach (var child in parsedObject.content)
                    Visit(child, sb);
            // process node
            var inkAtomsStory = target as InkAtomsStory;
            var prefix = parsedObject.debugMetadata != null
                ? $"{parsedObject.debugMetadata.fileName}:{parsedObject.debugMetadata.startLineNumber} "
                : "";
            if (parsedObject is Tag tag) _insideTag = tag.isStart;
            if (_insideTag)
            {
                if (parsedObject is Text t)
                    // check tag syntax
                    if (inkAtomsStory.HasTagErrors(t.text.Trim(), out var error))
                    {
                        sb.Append(prefix);
                        sb.AppendLine(error);
                    }
            }
            else if (parsedObject is Text text && text.text.Trim() != inkAtomsStory.commandLinePrefix &&
                     text.text.Trim().StartsWith(inkAtomsStory.commandLinePrefix))
            {
                // check text syntax
                if (inkAtomsStory.HasTextErrors(text.text.Trim(), out var error))
                {
                    sb.Append(prefix);
                    sb.AppendLine(error);
                }
            }
        }

        private static Story ParseStory(DefaultAsset file, StringBuilder sb)
        {
            var mainFilePath = NormalizePath(GetPath(file));
            FileHandler fileHandler = new(mainFilePath);
            var text = fileHandler.LoadInkFileContents(mainFilePath);
            Compiler compiler = new(
                text,
                new Compiler.Options
                {
                    sourceFilename = mainFilePath,
                    errorHandler = (message, type) =>
                    {
                        if (type != ErrorType.Author) sb.AppendLine(message);
                    },
                    fileHandler = fileHandler
                });
            var parsedStory = compiler.Parse();
            return parsedStory;
        }

        /// <summary>
        ///     Make the path of an ink file relative to the main file. If the path is not
        ///     fully qualified, then it's not changed.
        ///     E.g.:
        ///     <code>
        /// var includeName =
        ///     "c:\\chapter1\\included.ink";
        /// var relativeName = includeName
        ///     .MakeInkPathRelative("c:\\main.ink")
        /// relativeName == "chapter1\\included.ink"
        /// </code>
        ///     and
        ///     <code>
        /// var includeName =
        ///     "chapter1\\included.ink";
        /// var relativeName = includeName
        ///     .MakeInkPathRelative("c:\\main.ink")
        /// relativeName == "chapter1\\included.ink"
        /// </code>
        /// </summary>
        /// <param name="includeName"></param>
        /// <param name="mainFilePath"></param>
        /// <returns></returns>
        public static string MakeInkPathRelative(
            string includeName,
            string mainFilePath)
        {
            var mainFileDirectory = Path.GetDirectoryName(mainFilePath);
            if (Path.IsPathFullyQualified(includeName))
            {
                Assert.IsTrue(includeName.StartsWith(mainFileDirectory));
                return includeName[(mainFileDirectory.Length + 1)..];
            }

            return includeName;
        }

        /// <summary>
        ///     Resolve the path of an included ink file relative to the main project file.
        /// </summary>
        /// <param name="includeName">The path of the included file.</param>
        /// <param name="mainFilePath">The full path of the main project file.</param>
        /// <returns>The full, normalized path of the included ink file.</returns>
        public static string ResolveInkFilename(
            string includeName,
            string mainFilePath)
        {
            return Path.IsPathFullyQualified(includeName)
                ? includeName
                : NormalizePath(Path.Combine(
                    Path.GetDirectoryName(mainFilePath),
                    includeName
                ));
        }

        /// <summary>
        ///     Produce a normalize and unique version of a System.IO.Path.
        /// </summary>
        /// <param name="path">the path to normalize</param>
        /// <returns>The normalized System.IO.Path.</returns>
        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                .TrimEnd(
                    Path.DirectorySeparatorChar,
                    Path.AltDirectorySeparatorChar);
        }

        public static string GetPath(UnityEngine.Object assetObject)
        {
            var fileAssetPath = AssetDatabase.GetAssetPath(
                assetObject);
            var completePath = Path.Combine(
                Application.dataPath, "..", fileAssetPath);
            return completePath;
        }

        private class FileHandler : IFileHandler
        {
            private readonly string mainFilePath;

            public FileHandler(string mainFilePath)
            {
                this.mainFilePath = mainFilePath;
            }

            public string LoadInkFileContents(string fullFilename)
            {
                var content = File.ReadAllText(fullFilename);
                //var filename = MakeInkPathRelative(fullFilename, mainFilePath);
                return content;
            }

            public string ResolveInkFilename(string includeName)
            {
                return InkStoryEditor.ResolveInkFilename(includeName, mainFilePath);
            }
        }
    }
}