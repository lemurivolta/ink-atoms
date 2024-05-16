using Ink;
using UnityEditor;
using System.Text;

using UnityEngine;
using Ink.Parsed;
using UnityEngine.Assertions;
using System.IO;
using static UnityEngine.GraphicsBuffer;

namespace LemuRivolta.InkAtoms.Editor
{
    public class StoryExaminator
    {
        private bool insideTag;

        public abstract class Visitor
        {
            public abstract void Visited(Ink.Parsed.Object o, bool insideTag);
        }

        public void StartVisit(DefaultAsset file, ErrorHandler errorHandler, Visitor visitor)
        {
            var story = ParseStory(file, errorHandler);
            Visit(story, visitor);
        }

        private void Visit(Ink.Parsed.Object parsedObject, Visitor visitor)
        {
            // visit recursively
            if (parsedObject.content != null)
            {
                foreach (var child in parsedObject.content)
                {
                    Visit(child, visitor);
                }
            }
            // process node
            if (parsedObject is Tag tag)
            {
                insideTag = tag.isStart;
            }
            visitor.Visited(parsedObject, insideTag);
        }

        private Story ParseStory(DefaultAsset file, ErrorHandler errorHandler)
        {
            string mainFilePath = NormalizePath(GetPath(file));
            FileHandler fileHandler = new(mainFilePath);
            var text = fileHandler.LoadInkFileContents(mainFilePath);
            Compiler compiler = new(
                text,
                new Compiler.Options()
                {
                    sourceFilename = mainFilePath,
                    errorHandler = errorHandler,
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
