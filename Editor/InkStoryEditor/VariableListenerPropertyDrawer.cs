using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Ink.Parsed;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor
{
    [CustomPropertyDrawer(typeof(VariableListener))]
    public class VariableListenerPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Packages/it.lemurivolta.ink-atoms/Editor/InkStoryEditor/VariableListenerPropertyDrawer.uxml");
            var root = visualTreeAsset.CloneTree();

            var matchNameContainer = root.Q<VisualElement>("match-name-container");
            var matchNameName = root.Q<DropdownField>("match-name-name");
            var matchRegexContainer = root.Q<VisualElement>("match-regex-container");
            var matchListContainer = root.Q<VisualElement>("match-list-container");
            var matchNameInkListContainer = root.Q<VisualElement>("match-nameinklist-container");
            var inklistName = root.Q<DropdownField>("nameinklist-name");
            var regexError = root.Q<HelpBox>("regex-error");
            var regexList = root.Q<Label>("regex-list");
            var listVars = root.Q<ListView>("list-vars");

            SerializedProperty matchKindProperty = property.FindPropertyRelative("matchKind");
            SerializedProperty regexProperty = property.FindPropertyRelative("regex");

            var file = (property.serializedObject.targetObject as InkAtomsStory).SyntaxCheckFiles[0];
            var variableNames = GetVariableNames(file);

            root.Q<PropertyField>("match-kind-property-field").RegisterValueChangeCallback(vce =>
            {
                matchNameName.choices = variableNames;
                inklistName.choices = variableNames;
                var matchKind = (MatchKind)matchKindProperty.enumValueIndex;
                matchNameContainer.style.display = FromMatchKind(matchKind, MatchKind.Name);
                matchRegexContainer.style.display = FromMatchKind(matchKind, MatchKind.RegularExpression);
                matchListContainer.style.display = FromMatchKind(matchKind, MatchKind.List);
                matchNameInkListContainer.style.display = FromMatchKind(matchKind, MatchKind.NameInkList);
                OnRegexFieldBlur(null);
            });

            void OnRegexFieldBlur(FocusOutEvent _)
            {
                var regexValue = regexProperty.stringValue;
                try
                {
                    if (string.IsNullOrWhiteSpace(regexValue))
                    {
                        throw new System.Exception("Cannot be empty");
                    }
                    var regex = new Regex(regexValue);
                    regexError.style.display = DisplayStyle.None;

                    regexList.text = "matching: " + string.Join(
                        ", ",
                        variableNames.Where(vn => regex.IsMatch(vn)));
                }
                catch (System.Exception e)
                {
                    regexError.style.display = DisplayStyle.Flex;
                    regexError.text = e.Message;
                    regexList.text = "";
                }
            }
            root.Q<PropertyField>("regex-field").RegisterCallback<FocusOutEvent>(OnRegexFieldBlur);

            listVars.makeItem = () =>
            {
                var listVariableItem = new ListVariableItem
                {
                    Choices = variableNames
                };
                return listVariableItem;
            };

            return root;
        }

        private static DisplayStyle FromMatchKind(MatchKind matchKind, MatchKind wanted) =>
            matchKind == wanted ? DisplayStyle.Flex : DisplayStyle.None;

        private List<string> GetVariableNames(DefaultAsset file)
        {
            VariableVisitor variableVisitor = new();
            new StoryExaminator().StartVisit(file, (message, level) =>
            {
                switch (level)
                {
                    case Ink.ErrorType.Author: Debug.Log(message); break;
                    case Ink.ErrorType.Warning: Debug.LogWarning(message); break;
                    case Ink.ErrorType.Error: Debug.LogError(message); break;
                }
            }, variableVisitor);
            return variableVisitor.VariableNames;
        }

        private class VariableVisitor : StoryExaminator.Visitor
        {
            public List<string> VariableNames { get; private set; } = new();
            public override void Visited(Ink.Parsed.Object o, bool insideTag)
            {
                if (o is VariableAssignment variableAssignment && variableAssignment.isGlobalDeclaration)
                {
                    VariableNames.Add(variableAssignment.variableName);
                }
            }
        }
    }
}
