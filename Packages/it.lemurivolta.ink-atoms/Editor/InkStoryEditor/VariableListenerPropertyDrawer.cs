using System;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor
{
    [CustomPropertyDrawer(typeof(VariableListener))]
    public class VariableListenerPropertyDrawer : PropertyDrawer
    {
        private VisualElement matchNameContainer;
        private VisualElement matchRegexContainer;
        private VisualElement matchListContainer;
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Packages/it.lemurivolta.ink-atoms/Editor/InkStoryEditor/VariableListenerPropertyDrawer.uxml");
            var root = visualTreeAsset.CloneTree();

            root.BindProperty(property);

            matchNameContainer = root.Q<VisualElement>("match-name-container");
            matchRegexContainer = root.Q<VisualElement>("match-regex-container");
            matchListContainer = root.Q<VisualElement>("match-list-container");

            root.Q<PropertyField>("match-kind-property-field").RegisterValueChangeCallback(vce =>
            {
                var matchKind = (MatchKind)property.FindPropertyRelative("MatchKind").enumValueIndex;
                UpdateVisibility(matchKind);
            });

            return root;
        }

        private void UpdateVisibility(MatchKind matchKind)
        {
            matchNameContainer.style.display = matchKind == MatchKind.Name ? DisplayStyle.Flex : DisplayStyle.None;
            matchRegexContainer.style.display = matchKind == MatchKind.RegularExpression ? DisplayStyle.Flex : DisplayStyle.None;
            matchListContainer.style.display = matchKind == MatchKind.List ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
