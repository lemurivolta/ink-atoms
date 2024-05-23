using System;
using System.Collections.Generic;
using System.Linq;

using UnityAtoms.Editor;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor
{
    public class ExternalFunctionListField : VisualElement
    {
        private readonly VisualTreeAsset visualTreeAsset = default;

        public new class UxmlFactory : UxmlFactory<ExternalFunctionListField, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public UxmlTraits()
            {
                focusable.defaultValue = true;
            }
        }

        private readonly ListView listView;
        private readonly Button removeButton;

        private readonly TemplateContainer rootVisualElement;

        private const string CreateLabel = "Create new...";

        public ExternalFunctionListField()
        {
            if (visualTreeAsset == null)
            {
                visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Packages/it.lemurivolta.ink-atoms/Editor/InkStoryEditor/ExternalFunctionListField.uxml");
            }
            Assert.IsNotNull(visualTreeAsset);

            rootVisualElement = visualTreeAsset.CloneTree();
            listView = rootVisualElement.Q<ListView>("listview");
            listView.selectionChanged += ListView_selectionChanged;
            Assert.IsNotNull(listView);
            listView.makeItem = () => new ScriptableObjectListElement(typeof(BaseExternalFunction));

            var createButton = rootVisualElement.Q<DropdownField>("create-button");
            Assert.IsNotNull(createButton);
            // todo: removed types already installed
            types = TypeCache.GetTypesDerivedFrom<BaseExternalFunction>()
                .Where(t => !t.IsAbstract)
                .ToList();
            var choices = new List<string>();
            choices.AddRange(types.Select(t => t.Name));
            createButton.choices = choices;
            createButton.value = CreateLabel;
            createButton.RegisterValueChangedCallback(value =>
            {
                // check selected object
                var index = choices.IndexOf(value.newValue);
                if (index < 0)
                {
                    // "+" case
                    return;
                }

                // extract type
                Assert.IsTrue(index < types.Count);
                var type = types[index];

                // create the SO
                var so = ScriptableObject.CreateInstance(type);
                var path = AssetDatabase.GetAssetPath(serializedObject.targetObject);
                var pathParts = path.Split('/');
                pathParts[^1] = $"{type.Name}.asset";
                var assetPath = string.Join('/', pathParts);
                AssetDatabase.CreateAsset(so, assetPath);
                AssetDatabase.SaveAssets();

                // add it to the list
                var arrayProperty = serializedObject.FindProperty("externalFunctions.Array");
                arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
                arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1).objectReferenceValue = so;
                serializedObject.ApplyModifiedProperties();

                // reset the add button
                createButton.value = CreateLabel;
            });

            var addButton = rootVisualElement.Q<Button>("add-button");
            Assert.IsNotNull(addButton);
            addButton.RegisterCallback<ClickEvent>(OnAddButtonClick);

            removeButton = rootVisualElement.Q<Button>("remove-button");
            Assert.IsNotNull(removeButton);
            removeButton.RegisterCallback<ClickEvent>(OnRemoveButtonClick);
            UpdateRemoveButtonVisibility();

            Add(rootVisualElement);

            focusable = true;
        }

        private void OnAddButtonClick(ClickEvent evt)
        {
            var arrayProperty = serializedObject.FindProperty("externalFunctions.Array");
            arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
            arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1).objectReferenceValue = null;
            serializedObject.ApplyModifiedProperties();
        }

        private readonly List<Type> types;

        private void OnRemoveButtonClick(ClickEvent evt)
        {
            var index = listView.selectedIndex;
            Assert.IsTrue(index >= 0);
            var arrayProperty = serializedObject.FindProperty("externalFunctions.Array");
            arrayProperty.RemoveArrayElement(index);
            serializedObject.ApplyModifiedProperties();
        }

        private void ListView_selectionChanged(IEnumerable<object> _obj)
        {
            UpdateRemoveButtonVisibility();
        }

        private void UpdateRemoveButtonVisibility()
        {
            removeButton.SetEnabled(listView.selectedIndex >= 0);
        }

        private List<SerializedProperty> serializedProperties;
        private SerializedObject serializedObject;

        public void Setup(SerializedObject serializedObject)
        {
            SetupList(serializedObject);

            var prop = serializedObject.FindProperty("externalFunctions.Array");
            prop.Next(true);
            rootVisualElement.TrackPropertyValue(prop, p => SetupList(serializedObject));
        }

        public void SetupList(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
            serializedProperties = new();

            var arrayProperty = serializedObject.FindProperty("externalFunctions.Array");
            var endProperty = arrayProperty.GetEndProperty();
            arrayProperty.NextVisible(true);
            var index = 0;
            do
            {
                if (SerializedProperty.EqualContents(arrayProperty, endProperty))
                {
                    break;
                }
                if (arrayProperty.propertyType == SerializedPropertyType.ArraySize)
                {
                    continue;
                }
                serializedProperties.Add(serializedObject.FindProperty(arrayProperty.propertyPath));
                index++;
            } while (arrayProperty.NextVisible(false));

            listView.itemsSource = serializedProperties;
            listView.bindItem = (ve, idx) =>
            {
                var scriptableObjectListElement = ve as ScriptableObjectListElement;
                scriptableObjectListElement.Index = idx;
                scriptableObjectListElement.BindProperty(serializedProperties[idx]);
            };
        }
    }

}
