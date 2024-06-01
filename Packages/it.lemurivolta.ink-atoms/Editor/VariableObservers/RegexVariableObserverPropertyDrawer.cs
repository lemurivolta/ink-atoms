using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LemuRivolta.InkAtoms.VariableObserver;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor.Editor.VariableObservers
{
    [CustomPropertyDrawer(typeof(RegexVariableObserver))]
    public class RegexVariableObserverPropertyDrawer : PropertyDrawer
    {
        private InkAtomsStory _inkAtomsStory;
        private List<string> _variableNames;
        private HelpBox _regexError;
        private Label _regexList;
        private SerializedProperty _regexProperty;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // create the root visual element from uxml
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Packages/it.lemurivolta.ink-atoms/Editor/VariableObservers/RegexVariableObserverPropertyDrawer.uxml");
            var root = visualTreeAsset.CloneTree();

            // get the InkAtomsStory that this property drawer is bound to
            _inkAtomsStory = property.serializedObject.targetObject as InkAtomsStory;
            Assert.IsNotNull(_inkAtomsStory);

            // get the list of all variable names
            _variableNames = InkInspectorHelper.GetVariableNames(_inkAtomsStory.MainInkFile);

            // set up the regex validation and expansion
            _regexError = root.Q<HelpBox>("regex-error");
            _regexList = root.Q<Label>("regex-list");
            _regexProperty = property.FindPropertyRelative("regex");
            root.Q<PropertyField>("regex-field").RegisterCallback<FocusOutEvent>(OnRegexFieldBlur);
            OnRegexFieldBlur(null);

            // returned the completed tree
            return root;
        }

        private void OnRegexFieldBlur(FocusOutEvent _)
        {
            var regexValue = _regexProperty.stringValue;
            try
            {
                if (string.IsNullOrWhiteSpace(regexValue))
                {
                    throw new System.Exception("Cannot be empty");
                }

                var regex = new Regex(regexValue);
                _regexError.style.display = DisplayStyle.None;

                _regexList.text = "matching: " + string.Join(
                    ", ",
                    _variableNames.Where(vn => regex.IsMatch(vn)));
            }
            catch (System.Exception e)
            {
                _regexError.style.display = DisplayStyle.Flex;
                _regexError.text = e.Message;
                _regexList.text = "";
            }
        }
    }
}