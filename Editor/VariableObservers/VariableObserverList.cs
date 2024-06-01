using System;
using System.Linq;
using LemuRivolta.InkAtoms.VariableObserver;
using UnityAtoms.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor.Editor.VariableObservers
{
    public class VariableObserverList : VisualElement
    {
        private readonly ListView _listView;

        private readonly ObserverEntry[] _observerEntries =
        {
            new("Int Variable", typeof(IntVariableObserver)),
            new("String Variable", typeof(StringVariableObserver)),
            new("List Variable", typeof(ListVariableObserver)),
            new("Bool Variable", typeof(BoolVariableObserver)),
            new("Float Variable", typeof(FloatVariableObserver)),
            new("Listing", typeof(ListingVariableObserver)),
            new("Regex", typeof(RegexVariableObserver))
        };

        private SerializedObject _serializedObject;

        public VariableObserverList()
        {
            // get the asset containing the UXML for this component
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Packages/it.lemurivolta.ink-atoms/Editor/VariableObservers/VariableObserverList.uxml");
            Assert.IsNotNull(visualTreeAsset);

            // create a visual element from the asset
            var rootVisualElement = visualTreeAsset.CloneTree();

            // save a reference to the list view
            _listView = rootVisualElement.Q<ListView>("list-view");

            // handle the "create variable observer" button
            var createButton = rootVisualElement.Q<DropdownField>("create-button");
            createButton.choices.AddRange(from entry in _observerEntries select entry.Label);
            createButton.RegisterValueChangedCallback(value =>
            {
                // check selected object
                var index = createButton.choices.IndexOf(value.newValue);
                if (index == 0)
                    // "create variable observer" case: nothing to do
                    return;

                // create the observer
                var observer = _observerEntries[index - 1]
                    .ObserverType
                    .GetConstructor(Array.Empty<Type>())
                    ?.Invoke(Array.Empty<object>());
                Assert.IsNotNull(observer);
                Debug.Log(
                    $"Selecting index {index} - {_observerEntries[index - 1].Label} - created an observer of type {observer.GetType()}");

                // add it
                Assert.IsNotNull(_serializedObject);
                ArrayProperty.InsertArrayElementAtIndex(ArrayProperty.arraySize);
                ArrayProperty.GetArrayElementAtIndex(ArrayProperty.arraySize - 1).managedReferenceValue = observer;
                _serializedObject.ApplyModifiedProperties();

                // reset back to "create variable observer"
                createButton.value = createButton.choices[0];
            });

            // handle the remove button
            var removeButton = rootVisualElement.Q<Button>("remove-button");
            Assert.IsNotNull(removeButton);
            removeButton.RegisterCallback<ClickEvent>(_ =>
            {
                // we have to unbind and re-bind the list because of a strange
                // bug: somehow the listview breaks internally in reaction to
                // the removal of a property because it tries to access info
                // about the property itself that has been removed
                var index = _listView.selectedIndex;
                _listView.Unbind();
                Assert.IsNotNull(_serializedObject);
                if (index < 0) return;
                ArrayProperty.RemoveArrayElement(index);
                _serializedObject.ApplyModifiedProperties();
                _listView.Bind(_serializedObject);
            });

            // add the created visual elements in the component root
            Add(rootVisualElement);
        }

        private SerializedProperty ArrayProperty =>
            _serializedObject.FindProperty("variableObservers.Array");

        public void Setup(SerializedObject newSerializedObject)
        {
            _serializedObject = newSerializedObject;
            _listView.bindingPath = "variableObservers";
            _listView.Bind(newSerializedObject);
        }

        /// <summary>
        ///     A record-like struct to keep info about kinds of observers.
        /// </summary>
        private struct ObserverEntry
        {
            public readonly string Label;
            public readonly Type ObserverType;

            public ObserverEntry(string label, Type observerType)
            {
                Label = label;
                ObserverType = observerType;
            }
        }

        private new class UxmlFactory : UxmlFactory<VariableObserverList, UxmlTraits>
        {
        }

        private new class UxmlTraits : VisualElement.UxmlTraits
        {
        }
    }
}