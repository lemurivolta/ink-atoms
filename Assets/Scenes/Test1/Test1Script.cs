using System.Linq;
using Ink.Runtime;
using LemuRivolta.InkAtoms;
using LemuRivolta.InkAtoms.VariableObserver;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UIElements;

public class Test1Script : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private TextAsset inkTextAsset;
    [SerializeField] private InkAtomsStory inkAtomsStory;
    [SerializeField] private StringEvent continueEvent;
    [SerializeField] private ChosenChoiceEvent chosenChoiceEvent;

    [SerializeField] private SerializableInkListItemValueList testList;
    private Button[] choices;
    private VisualElement choicesRoot;
    private VisualElement continueRoot;
    private VisualElement logsContainer;

    private VisualElement root;

    private void Start()
    {
        var r = document.rootVisualElement;
        logsContainer = r.Q<VisualElement>("LogsContainer");
        root = r.Q<VisualElement>("TextRoot");
        continueRoot = r.Q<VisualElement>("ContinueContainer");
        continueRoot.Children().Cast<Button>().First().clicked += OnContinue;
        choicesRoot = r.Q<VisualElement>("ChoicesContainer");
        choices = choicesRoot.Children().Cast<Button>().ToArray();
        for (var i = 0; i < choices.Length; i++) BindChoice(choices[i], i);

        inkAtomsStory.StartStory(inkTextAsset);
        continueEvent.Raise(null);
    }

    private void BindChoice(Button button, int i)
    {
        button.clicked += () =>
        {
            continueRoot.visible = false;
            choicesRoot.visible = false;
            chosenChoiceEvent.Raise(new ChosenChoice
            {
                FlowName = null,
                ChoiceIndex = i
            });
        };
    }

    private void OnContinue()
    {
        continueRoot.visible = false;
        choicesRoot.visible = false;
        continueEvent.Raise(null);
    }

    public void OnStoryStep(StoryStep step)
    {
        if (!string.IsNullOrEmpty(step.Text))
        {
            var ve = new VisualElement();
            ve.Add(new Label { text = step.Text });
            root.Add(ve);
        }

        for (var i = 0; i < choices.Length; i++)
        {
            choices[i].visible = i < step.Choices.Length;
            if (choices[i].visible) choices[i].text = step.Choices[i].Text;
        }

        continueRoot.visible = step.CanContinue;
        choicesRoot.visible = step.Choices != null;
    }

    public void Var1Changed(IntPair pair)
    {
        var (curr, prev) = pair;
        logsContainer.Add(new Label
        {
            text = $"var1 changed: var1 went from {prev} to {curr}"
        });
    }

    public void VarXChanged(VariableChange change)
    {
        logsContainer.Add(new Label
        {
            text =
                $"varX changed: {change.Name} went from {change.OldValue?.valueObject} to {change.NewValue.valueObject}"
        });
    }

    public void Var1Or3Changed(VariableChange change)
    {
        logsContainer.Add(new Label
        {
            text =
                $"var1or3 changed: {change.Name} went from {change.OldValue?.valueObject} to {change.NewValue.valueObject}"
        });
    }

    public void TestListAdded(SerializableInkListItem item)
    {
        InkListItem i = item;
        logsContainer.Add(new Label
        {
            text = $"list item {i.fullName} added; now the list is {ListToString(testList)}"
        });
    }

    public void TestListRemoved(SerializableInkListItem item)
    {
        InkListItem i = item;
        logsContainer.Add(new Label
        {
            text = $"list item {i.fullName} removed; now the list is {ListToString(testList)}"
        });
    }

    private string ListToString(SerializableInkListItemValueList testList)
    {
        return "[" + string.Join(", ", testList.Select(item => ((InkListItem)item).fullName)) + "]";
    }
}