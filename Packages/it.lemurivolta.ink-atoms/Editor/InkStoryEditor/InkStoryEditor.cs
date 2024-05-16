using System.IO;
using System.Text;

using Ink;
using Ink.Parsed;

using UnityEditor;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor
{
    [CustomEditor(typeof(InkAtomsStory))]
    public class InkStoryEditor : UnityEditor.Editor
    {
        [SerializeField]
        private VisualTreeAsset visualTreeAsset = default;

        public override VisualElement CreateInspectorGUI()
        {
            if (visualTreeAsset == null)
            {
                visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Packages/it.lemurivolta.ink-atoms/Editor/InkStoryEditor/InkStoryEditor.uxml");
            }
            var rootVisualElement = visualTreeAsset.CloneTree();

            var atomsFoldout = rootVisualElement.Q<Foldout>("atoms-foldout");
            atomsFoldout.value = false;

            var syntaxHelpBox = rootVisualElement.Q<HelpBox>("syntax-warnings");
            syntaxHelpBox.style.display = DisplayStyle.None;
            var syntaxCheckButton = rootVisualElement.Q<Button>("syntax-check-button");
            syntaxCheckButton.clicked += () =>
            {
                CheckSyntax(syntaxHelpBox);
            };

            return rootVisualElement;
        }

        private void CheckSyntax(HelpBox syntaxHelpBox)
        {
            var inkAtomsStory = target as InkAtomsStory;
            StringBuilder sb = new();
            foreach (var file in inkAtomsStory.SyntaxCheckFiles)
            {
                try
                {
                    CheckFileSyntax(file, sb);
                }
                catch (System.Exception e)
                {
                    sb.AppendLine(e.ToString());
                }
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
            Story parsedStory = ParseStory(file, sb);
            insideTag = false;
            Visit(parsedStory, sb);
        }

        private bool insideTag;

        private void Visit(Ink.Parsed.Object parsedObject, StringBuilder sb)
        {
            // visit recursively
            if (parsedObject.content != null)
            {
                foreach (var child in parsedObject.content)
                {
                    Visit(child, sb);
                }
            }
            // process node
            InkAtomsStory inkAtomsStory = target as InkAtomsStory;
            var prefix = parsedObject.debugMetadata != null ?
                $"{parsedObject.debugMetadata.fileName}:{parsedObject.debugMetadata.startLineNumber} " :
                "";
            if (parsedObject is Tag tag)
            {
                insideTag = tag.isStart;
            }
            if (insideTag)
            {
                if (parsedObject is Text t)
                {
                    // check tag syntax
                    if (inkAtomsStory.HasTagErrors(t.text.Trim(), out var error))
                    {
                        sb.Append(prefix);
                        sb.AppendLine(error);
                    }
                }
            }
            else if (parsedObject is Text text && text.text.Trim() != inkAtomsStory.CommandLinePrefix && text.text.Trim().StartsWith(inkAtomsStory.CommandLinePrefix))
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
            string mainFilePath = NormalizePath(GetPath(file));
            FileHandler fileHandler = new(mainFilePath);
            var text = fileHandler.LoadInkFileContents(mainFilePath);
            Compiler compiler = new(
                text,
                new Compiler.Options()
                {
                    sourceFilename = mainFilePath,
                    errorHandler = (string message, ErrorType type) =>
                    {
                        if (type != ErrorType.Author)
                        {
                            sb.AppendLine(message);
                        }
                    },
                    fileHandler = fileHandler,
                });
            var parsedStory = compiler.Parse();
            return parsedStory;
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

            public string ResolveInkFilename(string includeName) =>
                InkStoryEditor.ResolveInkFilename(includeName, mainFilePath);
        }

        /// <summary>
        /// Make the path of an ink file relative to the main file. If the path is not
        /// fully qualified, then it's not changed.
        /// E.g.:
        /// <code>
        /// var includeName =
        ///     "c:\\chapter1\\included.ink";
        /// var relativeName = includeName
        ///     .MakeInkPathRelative("c:\\main.ink")
        /// relativeName == "chapter1\\included.ink"
        /// </code>
        /// and
        /// <code>
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
            var mainFileDirectory = System.IO.Path.GetDirectoryName(mainFilePath);
            if (System.IO.Path.IsPathFullyQualified(includeName))
            {
                Assert.IsTrue(includeName.StartsWith(mainFileDirectory));
                return includeName[(mainFileDirectory.Length + 1)..];
            }
            else
            {
                return includeName;
            }
        }

        /// <summary>
        /// Resolve the path of an included ink file relative to the main project file.
        /// </summary>
        /// <param name="includeName">The path of the included file.</param>
        /// <param name="mainFilePath">The full path of the main project file.</param>
        /// <returns>The full, normalized path of the included ink file.</returns>
        public static string ResolveInkFilename(
            string includeName,
            string mainFilePath) =>
            System.IO.Path.IsPathFullyQualified(includeName) ?
                includeName :
                NormalizePath(System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(mainFilePath),
                    includeName
                ));

        /// <summary>
        /// Produce a normalize and unique version of a System.IO.Path.
        /// </summary>
        /// <param name="path">the path to normalize</param>
        /// <returns>The normalized System.IO.Path.</returns>
        public static string NormalizePath(string path) =>
            System.IO.Path.GetFullPath(new System.Uri(path).LocalPath)
                .TrimEnd(
                    System.IO.Path.DirectorySeparatorChar,
                    System.IO.Path.AltDirectorySeparatorChar);

        public static string GetPath(UnityEngine.Object assetObject)
        {
            var fileAssetPath = AssetDatabase.GetAssetPath(
                assetObject);
            var completePath = System.IO.Path.Combine(
                Application.dataPath, "..", fileAssetPath);
            return completePath;
        }
    }
}
