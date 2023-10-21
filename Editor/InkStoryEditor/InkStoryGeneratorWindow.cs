using System.IO;

using LemuRivolta.InkAtoms;
using UnityAtoms.BaseAtoms;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
public class InkStoryGeneratorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Assets/Create/Ink Story/Create New Ink Story", priority = 10)]
    public static void ShowExample()
    {
        InkStoryGeneratorWindow wnd = GetWindow<InkStoryGeneratorWindow>();
        wnd.titleContent = new GUIContent("Ink Atoms Story Generator");
    }

    private DefaultAsset defaultFolder;

    private ObjectField folderField;

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement visualElement = m_VisualTreeAsset.Instantiate();
        root.Add(visualElement);

        // update size
        var contentContainer = root.Q<VisualElement>("unity-content-container");
        contentContainer.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

        // folder field: initialize and attach to change event
        folderField = visualElement.Q<ObjectField>("FolderField");
        folderField.RegisterValueChangedCallback(FolderFieldValueChanged);

        var dirPath = Selection.assetGUIDs.Length == 0 ?
            $"Assets" :
            AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        if (!Directory.Exists(dirPath))
        {
            dirPath = Path.GetDirectoryName(dirPath);
        }
        defaultFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(dirPath);
        folderField.value = defaultFolder;

        // attach to create button
        var createButton = root.Q<Button>("CreateButton");
        createButton.clicked += CreateButton_clicked;
    }

    private void CreateButton_clicked()
    {
        var name = rootVisualElement.Q<TextField>("NameField").text;
        var storyStepEvent = CreateAsset<StoryStepEvent>($"{name} - Story Step Event");
        var continueEvent = CreateAsset<StringEvent>($"{name} - Continue Event");
        var choiceEvent = CreateAsset<ChosenChoiceEvent>($"{name} - Chosen Choice Event");
        var inkAtomsStory = CreateAsset<InkAtomsStory>(name);
        inkAtomsStory.SetupAsset(storyStepEvent, continueEvent, choiceEvent);
        EditorUtility.SetDirty(inkAtomsStory);

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = inkAtomsStory;

        Close();
    }

    private void OnGeometryChanged(GeometryChangedEvent evt)
    {
        var visualElement = (VisualElement)evt.currentTarget;

        var width = visualElement.resolvedStyle.width;
        var height = visualElement.resolvedStyle.height;
        if (float.IsNaN(width) || float.IsNaN(height)) { return; }

        maxSize = new(width, height);
        minSize = maxSize;
    }

    private void FolderFieldValueChanged(ChangeEvent<Object> evt)
    {
        var path = AssetDatabase.GetAssetPath(evt.newValue);
        if (!Directory.Exists(path))
        {
            folderField.value = defaultFolder;
        }
    }

    private T CreateAsset<T>(string name) where T : ScriptableObject
    {
        var rootPath = AssetDatabase.GetAssetPath(folderField.value);
        var filePath = $"{rootPath}/{name}.asset";

        filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);

        // Create InkAtomsStory
        T asset = CreateInstance<T>();

        // Create Asset
        AssetDatabase.CreateAsset(asset, filePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(asset));

        return asset;
    }
}
#endif