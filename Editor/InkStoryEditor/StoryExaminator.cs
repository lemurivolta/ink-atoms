using System;
using System.IO;
using Ink;
using Ink.Parsed;
using LemuRivolta.InkAtoms.Editor.Editor.InkStoryEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = Ink.Parsed.Object;
using Path = System.IO.Path;

namespace LemuRivolta.InkAtoms.Editor
{
    public class StoryExaminator
    {
        private bool insideTag;

        public void StartVisit(DefaultAsset file, ErrorHandler errorHandler, Visitor visitor)
        {
            var story = ParseStory(file, errorHandler);
            Visit(story, visitor);
        }

        private void Visit(Object parsedObject, Visitor visitor)
        {
            // visit recursively
            if (parsedObject.content != null)
                foreach (var child in parsedObject.content)
                    Visit(child, visitor);
            // process node
            if (parsedObject is Tag tag) insideTag = tag.isStart;
            visitor.Visited(parsedObject, insideTag);
        }

        private Story ParseStory(DefaultAsset file, ErrorHandler errorHandler)
        {
            var mainFilePath = NormalizePath(GetPath(file));
            FileHandler fileHandler = new(mainFilePath);
            var text = fileHandler.LoadInkFileContents(mainFilePath);
            Compiler compiler = new(
                text,
                new Compiler.Options
                {
                    sourceFilename = mainFilePath,
                    errorHandler = errorHandler,
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

        public abstract class Visitor
        {
            public abstract void Visited(Object o, bool insideTag);
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