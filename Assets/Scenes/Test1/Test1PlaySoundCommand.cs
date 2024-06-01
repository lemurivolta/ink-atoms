using System.Collections;
using LemuRivolta.InkAtoms;
using LemuRivolta.InkAtoms.CommandLineProcessors;
using UnityAtoms.BaseAtoms;
using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create playSound command line parser")]
public class Test1PlaySoundCommand : CoroutineCommandLineProcessor
{
    [SerializeField] private GameObjectVariable audioPlayerVariable;
    [SerializeField] private InkAtomsStoryVariable inkStoryAtomsInitializedVariable;
    [SerializeField] private VariableValueVariable durationVariable;

    public Test1PlaySoundCommand() : base("playSound")
    {
    }

    public override IEnumerator Process(CommandLineProcessorContext context)
    {
        if (!audioPlayerVariable.Value.TryGetComponent<AudioPlayer>(out var audioPlayer))
        {
            Debug.LogError("no audio player in scene");
            yield break;
        }

        var soundKey = context["soundName"];

        var inkAtomsStory = inkStoryAtomsInitializedVariable.Value;

        inkAtomsStory.Call("getSoundAssetName", out var soundName, soundKey);

        var duration = audioPlayer.Play(soundName);

        yield return new WaitForSeconds(duration);

        durationVariable.Value = durationVariable.Value.Update(duration);
    }
}