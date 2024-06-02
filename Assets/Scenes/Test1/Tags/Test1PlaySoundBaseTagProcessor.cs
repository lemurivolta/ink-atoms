using System.Collections;
using LemuRivolta.InkAtoms;
using LemuRivolta.InkAtoms.TagProcessors;
using UnityAtoms.BaseAtoms;
using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create playSound tag processor")]
public class Test1PlaySoundBaseTagProcessor : CoroutineTagProcessor
{
    [SerializeField] private GameObjectVariable audioPlayerVariable;
    [SerializeField] private InkAtomsStoryVariable inkStoryAtomsInitializedVariable;

    public Test1PlaySoundBaseTagProcessor() : base("play-sound")
    {
    }

    protected override IEnumerator Process(TagProcessorContext context)
    {
        if (!audioPlayerVariable.Value.TryGetComponent<AudioPlayer>(out var audioPlayer))
        {
            Debug.LogError("no audio player in scene");
            yield break;
        }

        var soundKey = (string)context[0];

        inkStoryAtomsInitializedVariable.Value.Call("getSoundAssetName", out var soundName, soundKey);

        var duration = audioPlayer.Play(soundName);
        yield return new WaitForSeconds(duration);
    }
}