#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Ink;
using Ink.Parsed;
using UnityEditor;
using UnityEngine;
using ListDefinition = Ink.Parsed.ListDefinition;
using Object = Ink.Parsed.Object;
using ValueType = Ink.Runtime.ValueType;
using VariableAssignment = Ink.Parsed.VariableAssignment;

namespace LemuRivolta.InkAtoms.Editor.Editor.VariableObservers
{
    public static class InkInspectorHelper
    {
        /// <summary>
        ///     Get the names of all the global variables in the given project.
        /// </summary>
        /// <param name="file">The file of the main ink atoms story.</param>
        /// <param name="valueType">The allowed types for the variables.</param>
        /// <returns>The names of all the global variables in the given project.</returns>
        public static List<string> GetVariableNames(DefaultAsset file, ValueType? valueType)
        {
            // if there is no file, then we have no variables at all
            if (!file) return new List<string>();

            // visit the ink project, accumulating variable names and types
            VariableVisitor variableVisitor = new();
            new StoryExaminator().StartVisit(file, (message, level) =>
            {
                switch (level)
                {
                    case ErrorType.Author:
                        Debug.Log(message);
                        break;
                    case ErrorType.Warning:
                        Debug.LogWarning(message);
                        break;
                    case ErrorType.Error:
                        Debug.LogError(message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }
            }, variableVisitor);
            return variableVisitor.Variables
                .Where(tuple => valueType == null || tuple.Type == valueType)
                .Select(tuple => tuple.Name)
                .ToList();
        }

        /// <summary>
        ///     A visitor that saves the name of global variables.
        /// </summary>
        private class VariableVisitor : StoryExaminator.Visitor
        {
            public readonly List<(string Name, ValueType Type)> Variables = new();

            public override void Visited(Object o, bool insideTag)
            {
                if (o is VariableAssignment { isGlobalDeclaration: true } variableAssignment)
                {
                    // find the ValueType of the variable
                    var assignedValue = variableAssignment.content[0];
                    ValueType? type = assignedValue switch
                    {
                        Number n => n.value switch
                        {
                            float => ValueType.Float,
                            bool => ValueType.Bool,
                            int => ValueType.Int,
                            _ => null
                        },
                        StringExpression => ValueType.String,
                        ListDefinition => ValueType.List,
                        _ => null
                    };

                    // type was unknown or not valid for our purposes (e.g.: divert)
                    if (type == null) return;

                    // save this entry
                    Variables.Add((variableAssignment.variableName, type.Value));
                }
            }
        }
    }
}