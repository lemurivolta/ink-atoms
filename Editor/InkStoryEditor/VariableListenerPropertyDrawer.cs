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
        private DropdownField setterKindDropdownField;
        private PropertyField variableChangeEventPropertyField;
        private PropertyField variableValuePropertyField;
        private VisualElement rootVariableSetter;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Packages/it.lemurivolta.ink-atoms/Editor/InkStoryEditor/VariableListenerPropertyDrawer.uxml");
            var root = visualTreeAsset.CloneTree();

            root.BindProperty(property);

            matchNameContainer = root.Q<VisualElement>("match-name-container");
            matchRegexContainer = root.Q<VisualElement>("match-regex-container");
            matchListContainer = root.Q<VisualElement>("match-list-container");
            setterKindDropdownField = root.Q<DropdownField>("setter-kind-dropdown-field");
            variableChangeEventPropertyField = root.Q<PropertyField>("variable-change-event-property-field");
            variableValuePropertyField = root.Q<PropertyField>("variable-value-property-field");
            rootVariableSetter = root.Q<VisualElement>("root-variable-setter");

            root.Q<PropertyField>("match-kind-property-field").RegisterValueChangeCallback(vce =>
            {
                var matchKind = (MatchKind)property.FindPropertyRelative("MatchKind").enumValueIndex;
                UpdateVisibility(matchKind);
            });

            SerializedProperty valueSetterKindProperty = property.FindPropertyRelative(nameof(VariableListener.ValueSetterKind));
            var currValue = (ValueSetterKind)valueSetterKindProperty.enumValueIndex;
            setterKindDropdownField.value = currValue == ValueSetterKind.Variable ? "V" : "E";
            if (currValue == ValueSetterKind.Variable)
            {
                rootVariableSetter.AddToClassList("variable-kind");
            }
            else
            {
                rootVariableSetter.RemoveFromClassList("variable-kind");
            }
            setterKindDropdownField.RegisterValueChangedCallback(vce =>
            {
                ValueSetterKind kind = vce.newValue == "V" ? ValueSetterKind.Variable : ValueSetterKind.Event;
                valueSetterKindProperty.enumValueIndex = (int)kind;
                if (kind == ValueSetterKind.Variable)
                {
                    rootVariableSetter.AddToClassList("variable-kind");
                }
                else
                {
                    rootVariableSetter.RemoveFromClassList("variable-kind");
                }
                property.serializedObject.ApplyModifiedProperties();
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
