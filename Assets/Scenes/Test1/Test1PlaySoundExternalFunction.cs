using System.Collections;

using LemuRivolta.InkAtoms;

using UnityAtoms;
using UnityAtoms.BaseAtoms;

using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create playSound external function")]
public class Test1PlaySoundExternalFunction : CoroutineExternalFunction
{
    [SerializeField] private GameObjectVariable audioPlayerVariable;
    [SerializeField] private InkAtomsStoryVariable inkStoryAtomsInitializedVariable;

    public Test1PlaySoundExternalFunction() : base("playSound")
    {
    }

    public override IEnumerator Call(ExternalFunctionContextWithResult context)
    {
        if (!audioPlayerVariable.Value.TryGetComponent<AudioPlayer>(out var audioPlayer))
        {
            Debug.LogError("no audio player in scene");
        }
        var soundKey = context.Arguments[0] as string;
        inkStoryAtomsInitializedVariable.Value.Call("getSoundAssetName", out var soundName, soundKey);
        var duration = audioPlayer.Play(soundName);
        context.ReturnValue = duration;
        yield return new WaitForSeconds(duration);
    }
}
