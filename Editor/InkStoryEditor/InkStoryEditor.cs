using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace LemuRivolta.InkAtoms.Editor
{
    [CustomEditor(typeof(InkAtomsStory))]
    public class InkStoryEditor : UnityEditor.Editor
    {
        [SerializeField]
        private VisualTreeAsset visualTreeAsset = default;

        public override VisualElement CreateInspectorGUI()
        {
            var rootVisualElement = visualTreeAsset.CloneTree();

            var atomsFoldout = rootVisualElement.Q<Foldout>("atoms-foldout");
            atomsFoldout.value = false;

            return rootVisualElement;
        }
    }
}
