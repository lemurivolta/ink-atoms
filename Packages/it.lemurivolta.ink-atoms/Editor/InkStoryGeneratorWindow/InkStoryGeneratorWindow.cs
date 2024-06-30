using System;
using System.IO;
using LemuRivolta.InkAtoms;
using UnityAtoms.BaseAtoms;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

#if UNITY_EDITOR

/// <summary>
///     The window used to generate a new ink atoms story together with all the main
///     needed atoms.
/// </summary>
public class InkStoryGeneratorWindow : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset;

    private DefaultAsset defaultFolder;

    private ObjectField folderField;

    private HelpBox folderHelpBox;

    private ObjectField mainInkFileField;

    private HelpBox mainInkFileHelpBox;

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        var root = rootVisualElement;

        // Instantiate UXML
        var visualElement = m_VisualTreeAsset.CloneTree();
        visualElement.style.flexGrow = 1;
        root.Add(visualElement);

        // update size
        //var contentContainer = root.Q<VisualElement>("unity-content-container");
        //contentContainer.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

        // folder field: initialize and attach to change event
        folderField = visualElement.Q<ObjectField>("FolderField");
        folderHelpBox = visualElement.Q<HelpBox>("folder-help-box");
        folderField.RegisterValueChangedCallback(FolderFieldValueChanged);

        // ink main file field: initialize and attach to change event
        mainInkFileField = visualElement.Q<ObjectField>("InkFileField");
        mainInkFileHelpBox = visualElement.Q<HelpBox>("main-ink-file-help-box");
        mainInkFileField.RegisterValueChangedCallback(MainInkFileFieldChanged);
        if (Selection.assetGUIDs.Length > 0)
            mainInkFileField.value = AssetDatabase.LoadAssetAtPath<Object>(
                AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]));

        // set the current folder as the root Assets folder if nothing was selected
        var dirPath = Selection.assetGUIDs.Length == 0
            ? "Assets"
            : AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        if (!Directory.Exists(dirPath)) dirPath = Path.GetDirectoryName(dirPath);
        defaultFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(dirPath);
        folderField.value = defaultFolder;

        // attach to create button
        var createButton = root.Q<Button>("CreateButton");
        createButton.clicked += CreateButton_clicked;
    }

    [MenuItem("Assets/Create/Ink Atoms/Story", priority = 10)]
    public static void ShowExample()
    {
        var wnd = GetWindow<InkStoryGeneratorWindow>();
        wnd.minSize = new Vector2(470, 300);
        wnd.titleContent = new GUIContent("Ink Atoms Story Generator");
    }

    private void MainInkFileFieldChanged(ChangeEvent<Object> evt)
    {
        var display = DisplayStyle.Flex;
        var messageType = HelpBoxMessageType.Warning;
        var text = "";
        if (evt.newValue is DefaultAsset asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            if (!File.Exists(path))
            {
                messageType = HelpBoxMessageType.Error;
                text = "Must set an ink file as main ink file";
            }
            else if (!path.EndsWith(".ink"))
            {
                messageType = HelpBoxMessageType.Warning;
                text = "Selected asset does not end in .ink";
            }
            else
            {
                display = DisplayStyle.None;
            }
        }
        else
        {
            messageType = HelpBoxMessageType.Error;
            text = "Must set an ink file as main ink file";
        }

        mainInkFileHelpBox.style.display = display;
        mainInkFileHelpBox.messageType = messageType;
        mainInkFileHelpBox.text = text;
    }

    private void CreateButton_clicked()
    {
        var name = rootVisualElement.Q<TextField>("NameField").text;
        var storyStepVariable = CreateAsset<StoryStepVariable>($"{name} - Story Step Variable");
        var continueEvent = CreateAsset<StringEvent>($"{name} - Continue Event",
            c => c.ReplayBufferSize = 0);
        var choiceEvent = CreateAsset<ChosenChoiceEvent>($"{name} - Chosen Choice Event",
            c => c.ReplayBufferSize = 0);
        var initializedInkAtomsStory =
            CreateAsset<InkAtomsStoryVariable>($"{name} - Initialized Ink Atoms Story Variable");
        var inkAtomsStory = CreateAsset<InkAtomsStory>(name);
        inkAtomsStory.SetupAsset(mainInkFileField.value as DefaultAsset, storyStepVariable, continueEvent, choiceEvent,
            initializedInkAtomsStory);
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
        if (float.IsNaN(width) || float.IsNaN(height)) return;

        maxSize = new Vector2(width, height);
        minSize = maxSize;
    }

    private void FolderFieldValueChanged(ChangeEvent<Object> evt)
    {
        var path = AssetDatabase.GetAssetPath(evt.newValue);
        if (!Directory.Exists(path))
        {
            folderHelpBox.messageType = HelpBoxMessageType.Error;
            folderHelpBox.text = "Must be a folder";
            folderHelpBox.style.display = DisplayStyle.Flex;
        }
        else
        {
            folderHelpBox.style.display = DisplayStyle.None;
        }
    }

    private T CreateAsset<T>(string name, Action<T> updateBeforeSave = null) where T : ScriptableObject
    {
        var rootPath = AssetDatabase.GetAssetPath(folderField.value);
        var filePath = $"{rootPath}/{name}.asset";

        filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);

        // Create the asset
        var asset = CreateInstance<T>();

        // Update it if needed
        updateBeforeSave?.Invoke(asset);

        // Save Asset on database 
        AssetDatabase.CreateAsset(asset, filePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(asset));

        return asset;
    }
}
#endif