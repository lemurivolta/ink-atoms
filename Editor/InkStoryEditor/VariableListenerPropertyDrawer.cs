using System.Text.RegularExpressions;

using UnityEditor;
using UnityEditor.UIElements;

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
            var matchRegexContainer = root.Q<VisualElement>("match-regex-container");
            var matchListContainer = root.Q<VisualElement>("match-list-container");
            var matchNameInkListContainer = root.Q<VisualElement>("match-nameinklist-container");
            var regexError = root.Q<HelpBox>("regex-error");

            SerializedProperty matchKindProperty = property.FindPropertyRelative("matchKind");
            SerializedProperty regexProperty = property.FindPropertyRelative("regex");

            root.Q<PropertyField>("match-kind-property-field").RegisterValueChangeCallback(vce =>
            {
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
                    new Regex(regexValue);
                    regexError.style.display = DisplayStyle.None;
                }
                catch (System.Exception e)
                {
                    regexError.style.display = DisplayStyle.Flex;
                    regexError.text = e.Message;
                }
            }
            root.Q<PropertyField>("regex-field").RegisterCallback<FocusOutEvent>(OnRegexFieldBlur);

            return root;
        }

        private static DisplayStyle FromMatchKind(MatchKind matchKind, MatchKind wanted) =>
            matchKind == wanted ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
