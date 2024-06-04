using System;
using System.Collections.Generic;
using System.Linq;
using UnityAtoms.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor.Editor.StrategyScriptableObjectListField
{
    /// <summary>
    ///     The class handling the implementation of the interface to show a list of
    ///     ScriptableObject to use through the Strategy pattern. It shows the list and
    ///     also implements a control to create instances of compatible classes on the
    ///     fly.
    /// </summary>
    public class StrategyScriptableObjectListField : VisualElement
    {
        private const string CreateLabel = "Create new...";

        private readonly TemplateContainer _rootVisualElement;
        private readonly VisualTreeAsset _visualTreeAsset;

        private readonly ListView listView;
        private readonly Button removeButton;

        private bool _defaultOpened;

        private string _label;

        private Type _strategyType;
        private SerializedObject serializedObject;

        private List<SerializedProperty> serializedProperties;

        private List<Type> types;

        public StrategyScriptableObjectListField()
        {
            if (_visualTreeAsset == null)
                _visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Packages/it.lemurivolta.ink-atoms/Editor/StrategyScriptableObjectListField/StrategyScriptableObjectListField.uxml");

            Assert.IsNotNull(_visualTreeAsset);

            _rootVisualElement = _visualTreeAsset.CloneTree();
            listView = _rootVisualElement.Q<ListView>("listview");
            listView.selectionChanged += ListView_selectionChanged;
            Assert.IsNotNull(listView);
            listView.makeItem = () => new ScriptableObjectListElement(strategyType);

            SetupCreateButton();

            var addButton = _rootVisualElement.Q<Button>("add-button");
            Assert.IsNotNull(addButton);
            addButton.RegisterCallback<ClickEvent>(OnAddButtonClick);

            removeButton = _rootVisualElement.Q<Button>("remove-button");
            Assert.IsNotNull(removeButton);
            removeButton.RegisterCallback<ClickEvent>(OnRemoveButtonClick);
            UpdateRemoveButtonVisibility();

            UpdateLabel();
            UpdateFoldoutOpened();

            Add(_rootVisualElement);

            focusable = true;
        }

        public Type strategyType
        {
            get => _strategyType;
            set
            {
                _strategyType = value;
                SetupCreateButton();
            }
        }

        public string fieldName { get; set; }

        public string label
        {
            get => _label;
            set
            {
                _label = value;
                UpdateLabel();
            }
        }

        public bool defaultOpened
        {
            get => _defaultOpened;
            set
            {
                _defaultOpened = value;
                UpdateFoldoutOpened();
            }
        }

        private void UpdateFoldoutOpened()
        {
            var foldout = _rootVisualElement?.Q<Foldout>("foldout");
            if (foldout != null) foldout.value = defaultOpened;
        }

        private void UpdateLabel()
        {
            var foldout = _rootVisualElement?.Q<Foldout>("foldout");
            if (foldout != null) foldout.text = label;
        }

        private void SetupCreateButton()
        {
            var createButton = _rootVisualElement.Q<DropdownField>("create-button");
            Assert.IsNotNull(createButton);
            // todo: removed types already installed
            types = strategyType == null
                ? new List<Type>()
                : TypeCache.GetTypesDerivedFrom(strategyType)
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
                    // "+" case
                    return;

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
                var arrayProperty = serializedObject.FindProperty($"{fieldName}.Array");
                arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
                arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1).objectReferenceValue = so;
                serializedObject.ApplyModifiedProperties();

                // reset the add button
                createButton.value = CreateLabel;
            });
        }

        private void OnAddButtonClick(ClickEvent evt)
        {
            var arrayProperty = serializedObject.FindProperty($"{fieldName}.Array");
            arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
            arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1).objectReferenceValue = null;
            serializedObject.ApplyModifiedProperties();
        }

        private void OnRemoveButtonClick(ClickEvent evt)
        {
            var index = listView.selectedIndex;
            Assert.IsTrue(index >= 0);
            var arrayProperty = serializedObject.FindProperty($"{fieldName}.Array");
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

        public void Setup(SerializedObject serializedObject)
        {
            SetupList(serializedObject);

            var prop = serializedObject.FindProperty($"{fieldName}.Array");
            prop.Next(true);
            _rootVisualElement.TrackPropertyValue(prop, p => SetupList(serializedObject));
        }

        public void SetupList(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
            serializedProperties = new List<SerializedProperty>();

            var arrayProperty = serializedObject.FindProperty($"{fieldName}.Array");
            var endProperty = arrayProperty.GetEndProperty();
            arrayProperty.NextVisible(true);
            var index = 0;
            do
            {
                if (SerializedProperty.EqualContents(arrayProperty, endProperty)) break;

                if (arrayProperty.propertyType == SerializedPropertyType.ArraySize) continue;

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

        public new class UxmlFactory : UxmlFactory<StrategyScriptableObjectListField, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription fieldName = new()
            {
                name = "field-name"
            };

            private readonly UxmlStringAttributeDescription label = new()
            {
                name = "label"
            };

            private readonly UxmlTypeAttributeDescription<ScriptableObject> strategyType =
                new()
                {
                    name = "strategy-type"
                };

            private UxmlBoolAttributeDescription defaultOpened = new()
            {
                name = "default-opened",
                defaultValue = false
            };

            public UxmlTraits()
            {
                focusable.defaultValue = true;
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var s = (StrategyScriptableObjectListField)ve;
                s.strategyType = strategyType.GetValueFromBag(bag, cc);
                s.fieldName = fieldName.GetValueFromBag(bag, cc);
                s.label = label.GetValueFromBag(bag, cc);
            }
        }
    }
}