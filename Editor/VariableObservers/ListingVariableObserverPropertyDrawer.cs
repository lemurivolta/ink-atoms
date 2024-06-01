using LemuRivolta.InkAtoms.VariableObserver;
using UnityEditor;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor.Editor.VariableObservers
{
    [CustomPropertyDrawer(typeof(ListingVariableObserver))]
    public class ListingVariableObserverPropertyDrawer : PropertyDrawer
    {
        private InkAtomsStory _inkAtomsStory;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // create the root visual element from uxml
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Packages/it.lemurivolta.ink-atoms/Editor/VariableObservers/ListingVariableObserverPropertyDrawer.uxml");
            var root = visualTreeAsset.CloneTree();

            // get the InkAtomsStory that this property drawer is bound to
            _inkAtomsStory = property.serializedObject.targetObject as InkAtomsStory;
            Assert.IsNotNull(_inkAtomsStory);

            // setup the listview's dropdown field so to show only valid variables
            var variableNames = InkInspectorHelper.GetVariableNames(_inkAtomsStory.MainInkFile, null);
            var listVars = root.Q<ListView>("list-vars");
            listVars.makeItem = () =>
            {
                var dropdown = new DropdownField
                {
                    choices = variableNames
                };
                var visualElement = new VisualElement();
                visualElement.AddToClassList("list-vars-dropdown");
                visualElement.Add(dropdown);
                return visualElement;
            };

            // returned the completed tree
            return root;
        }
    }
}