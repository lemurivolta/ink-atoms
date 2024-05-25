using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace LemuRivolta.InkAtoms.Editor
{
    // should be of ScriptableObject, but there's a limitation: https://forum.unity.com/threads/popupfield-binding-gives-error-field-type-is-not-compatible-with-property.846868/
    /// <summary>
    ///     An <see cref="ObjectField" /> whose label changes dinamically to reflect the
    ///     "Name" property of the object itself.
    /// </summary>
    public class ScriptableObjectListElement : BindableElement, INotifyValueChanged<Object>
    {
        public static string SelectedClass = "scriptableobjectlistelement-selected";
        private readonly Type baseType;
        private readonly FieldInfo nameFieldInfo;

        private readonly ObjectField objectField;
        private readonly VisualTreeAsset visualTreeAsset;

        private int index = -1;

        private ScriptableObject m_Value;

        public ScriptableObjectListElement(Type baseType)
        {
            this.baseType = baseType;
            nameFieldInfo = baseType.GetField("Name");

            if (visualTreeAsset == null)
                visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Packages/it.lemurivolta.ink-atoms/Editor/StrategyScriptableObjectListField/ScriptableObjectListElement.uxml");
            Assert.IsNotNull(visualTreeAsset);

            var rootVisualElement = visualTreeAsset.CloneTree();
            objectField = rootVisualElement.Q<ObjectField>("object-field");
            Assert.IsNotNull(objectField);
            objectField.objectType = baseType;
            objectField.RegisterValueChangedCallback(OnObjectFieldChanged);
            UpdateLabel();
            Assert.IsNotNull(objectField);
            Add(rootVisualElement);
        }

        public int Index
        {
            get => index;
            set
            {
                index = value;
                UpdateLabel();
            }
        }

        public Object value
        {
            get => m_Value;
            set
            {
                if (value == this.value) return;
                var previous = this.value;
                SetValueWithoutNotify(value);

                using var evt = ChangeEvent<Object>.GetPooled(previous, value);
                evt.target = this;
                SendEvent(evt);
            }
        }

        public void SetValueWithoutNotify(Object newValue)
        {
            if (newValue == null)
            {
                m_Value = null;
                objectField.SetValueWithoutNotify(null);
            }
            else if (newValue is ScriptableObject so && baseType.IsAssignableFrom(so.GetType()))
            {
                m_Value = so;
                objectField.SetValueWithoutNotify(so);
                UpdateLabel();
            }
            else
            {
                throw new ArgumentException($"expected a {baseType.Name}", nameof(newValue));
            }
        }

        private void OnObjectFieldChanged(ChangeEvent<Object> evt)
        {
            value = evt.newValue;
        }

        private void UpdateLabel()
        {
            if (objectField != null && nameFieldInfo != null && index >= 0)
                objectField.label = m_Value == null ? "[empty]" : nameFieldInfo.GetValue(m_Value) as string;
        }
    }
}