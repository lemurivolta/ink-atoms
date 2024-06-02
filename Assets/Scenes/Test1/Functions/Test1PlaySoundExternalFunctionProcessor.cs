using System.Collections;
using LemuRivolta.InkAtoms;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;
using UnityAtoms.BaseAtoms;
using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create playSound external function")]
public class Test1PlaySoundExternalFunctionProcessor : CoroutineExternalFunctionProcessor
{
    [SerializeField] private GameObjectVariable audioPlayerVariable;
    [SerializeField] private InkAtomsStoryVariable inkStoryAtomsInitializedVariable;

    public Test1PlaySoundExternalFunctionProcessor() : base("playSound")
    {
    }

    protected override IEnumerator Process(ExternalFunctionProcessorContextWithResult context)
    {
        if (!audioPlayerVariable.Value.TryGetComponent<AudioPlayer>(out var audioPlayer))
            Debug.LogError("no audio player in scene");
        var soundKey = context.Get<string>(0);
        inkStoryAtomsInitializedVariable.Value.Call("getSoundAssetName", out var soundName, soundKey);
        var duration = audioPlayer.Play(soundName);
        context.ReturnValue = duration;
        yield return new WaitForSeconds(duration);
    }
}