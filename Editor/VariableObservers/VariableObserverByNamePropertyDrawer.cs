using System;
using Ink.Runtime;
using LemuRivolta.InkAtoms.VariableObserver;
using UnityEditor;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using ValueType = Ink.Runtime.ValueType;

namespace LemuRivolta.InkAtoms.Editor.Editor.VariableObservers
{
    [CustomPropertyDrawer(typeof(VariableObserverByName<>), true)]
    public class VariableObserverByNamePropertyDrawer : PropertyDrawer
    {
        /// <summary>
        ///     The <see cref="ValueType" /> corresponding to the generic type of the <see cref="VariableObserverByName{T}" />
        ///     being managed by this property drawer.
        ///     E.g., <c>VariableObserverByName&lt;int&gt;</c> corresponds to <c>ValueType.Int</c>.
        /// </summary>
        private ValueType _correspondingValueType;

        /// <summary>
        ///     The parent <see cref="InkAtomsStory" /> that this property drawer is displaying.
        /// </summary>
        private InkAtomsStory _inkAtomsStory;

        private DropdownField _matchName;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // create the root visual element from uxml
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Packages/it.lemurivolta.ink-atoms/Editor/VariableObservers/VariableObserverByNamePropertyDrawer.uxml");
            var root = visualTreeAsset.CloneTree();

            // extract the ink atoms story this property belongs to
            _inkAtomsStory = property.serializedObject.targetObject as InkAtomsStory;
            Assert.IsNotNull(_inkAtomsStory);

            // extract the type for this instance
            var managedType = GetManagedType(property);
            if (managedType == typeof(int))
                _correspondingValueType = ValueType.Int;
            else if (managedType == typeof(float))
                _correspondingValueType = ValueType.Float;
            else if (managedType == typeof(string))
                _correspondingValueType = ValueType.String;
            else if (managedType == typeof(bool))
                _correspondingValueType = ValueType.Bool;
            else if (managedType == typeof(InkList))
                _correspondingValueType = ValueType.List;
            else
                throw new Exception($"Unknown managed type {managedType.FullName}");

            // get a reference to the dropdown with all the fields
            _matchName = root.Q<DropdownField>("match-name");

            // connect to the "allow all variables" toggle and immediately update the dropdown choices
            var allowAllToggle = root.Q<Toggle>("allow-all-variables-toggle");
            allowAllToggle.RegisterValueChangedCallback(ev => UpdateChoices(allowAllToggle.value));
            UpdateChoices(allowAllToggle.value);

            // returned the completed tree
            return root;
        }

        /// <summary>
        ///     Update the list of choices in the variables dropdown.
        /// </summary>
        /// <param name="allowAll">
        ///     If <c>true</c>, all variables are displayed; otherwise, only those whose
        ///     guessed type can be handled by this variable.
        /// </param>
        private void UpdateChoices(bool allowAll)
        {
            // get the list of variable names from the main ink file
            var file = _inkAtomsStory.mainInkFile;
            var variableNames = InkInspectorHelper.GetVariableNames(file, allowAll ? null : _correspondingValueType);

            // force the names to only belong to the given list of variable names
            _matchName.choices = variableNames;
        }

        private Type GetManagedType(SerializedProperty property)
        {
            var type = property.managedReferenceValue.GetType();
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(VariableObserverByName<>))
                    return type.GenericTypeArguments[0];

                type = type.BaseType;
            }

            throw new Exception("cannot find managed type for simple type variable");
        }
    }
}