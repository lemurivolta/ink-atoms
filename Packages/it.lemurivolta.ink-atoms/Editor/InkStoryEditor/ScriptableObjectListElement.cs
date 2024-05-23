using System.Reflection;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor
{
    // should be of ScriptableObject, but there's a limitation: https://forum.unity.com/threads/popupfield-binding-gives-error-field-type-is-not-compatible-with-property.846868/
    public class ScriptableObjectListElement : BindableElement, INotifyValueChanged<Object>
    {
        private readonly VisualTreeAsset visualTreeAsset = default;

        private readonly ObjectField objectField;

        public static string SelectedClass = "scriptableobjectlistelement-selected";

        private int index = -1;
        private readonly System.Type baseType;
        private readonly PropertyInfo namePropertyInfo;

        public ScriptableObjectListElement(System.Type baseType)
        {
            this.baseType = baseType;
            namePropertyInfo = baseType.GetProperty("Name");

            if (visualTreeAsset == null)
            {
                visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Packages/it.lemurivolta.ink-atoms/Editor/InkStoryEditor/ScriptableObjectListElement.uxml");
            }
            Assert.IsNotNull(visualTreeAsset);

            var rootVisualElement = visualTreeAsset.CloneTree();
            objectField = rootVisualElement.Q<ObjectField>("object-field");
            Assert.IsNotNull(objectField);
            objectField.RegisterValueChangedCallback(OnObjectFieldChanged);
            UpdateLabel();
            Assert.IsNotNull(objectField);
            Add(rootVisualElement);

            var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                    "Packages/it.lemurivolta.ink-atoms/Editor/InkStoryEditor/ScriptableObjectListElement.uss");
            Assert.IsNotNull(stylesheet);
            styleSheets.Add(stylesheet);
        }

        private void OnObjectFieldChanged(ChangeEvent<Object> evt)
        {
            value = evt.newValue;
        }

        private void UpdateLabel()
        {
            if (objectField != null && index >= 0)
            {
                objectField.label = m_Value == null ? "[empty]" : namePropertyInfo.GetValue(m_Value) as string;
            }
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

        private ScriptableObject m_Value;

        public Object value
        {
            get => m_Value;
            set
            {
                if (value == this.value)
                {
                    return;
                }
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
                throw new System.ArgumentException($"expected a {baseType.Name}", nameof(newValue));
            }
        }
    }
}
