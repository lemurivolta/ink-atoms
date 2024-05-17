using System.Collections;
using System.Collections.Generic;

using LemuRivolta.InkAtoms;

using UnityAtoms;
using UnityAtoms.BaseAtoms;

using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create playSound tag processor")]
public class Test1PlaySoundTagProcessor : TagProcessor
{
    [SerializeField] private GameObjectVariable audioPlayerVariable;
    [SerializeField] private InkAtomsStoryVariable inkStoryAtomsInitializedVariable;

    public Test1PlaySoundTagProcessor() : base("play-sound") { }

    public override IEnumerator Process(TagProcessorContext context)
    {
        if (!audioPlayerVariable.Value.TryGetComponent<AudioPlayer>(out var audioPlayer))
        {
            Debug.LogError("no audio player in scene");
            yield break;
        }

        var soundKey = context.Parameters[0];

        inkStoryAtomsInitializedVariable.Value.Call("getSoundAssetName", out var soundName, soundKey);

        var duration = audioPlayer.Play(soundName);
        yield return new WaitForSeconds(duration);
    }
}
