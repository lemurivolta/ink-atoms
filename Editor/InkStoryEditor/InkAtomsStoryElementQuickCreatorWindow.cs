using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

public class InkAtomsStoryElementQuickCreatorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }

    public void BindTypes(IEnumerable<System.Type> types)
    {
        var listView = rootVisualElement.Q<ListView>();
        string[] typeNames = types.Select(t => t.Name).ToArray();
        listView.itemsSource = typeNames;
        listView.makeItem = () => new Label();
        listView.bindItem = (ve, idx) => (ve as Label).text = typeNames[idx];
    }
}
