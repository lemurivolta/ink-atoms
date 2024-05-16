using System.Collections;
using System.Collections.Generic;

using LemuRivolta.InkAtoms;

using UnityAtoms;
using UnityAtoms.BaseAtoms;

using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create playSound command line parser")]
public class Test1PlaySoundCommand : CommandLineParser
{
    [SerializeField] private GameObjectVariable audioPlayerVariable;
    [SerializeField] private InkAtomsStoryVariable inkStoryAtomsInitializedVariable;

    public Test1PlaySoundCommand() : base("playSound") { }

    public override IEnumerator Invoke(IDictionary<string, Parameter> parameters, StoryChoice[] _, CommandLineParserAction __)
    {
        if (!audioPlayerVariable.Value.TryGetComponent<AudioPlayer>(out var audioPlayer))
        {
            Debug.LogError("no audio player in scene");
            yield break;
        }

        var soundKey = GetParameter(parameters, "soundName");

        var inkAtomsStory = inkStoryAtomsInitializedVariable.Value;

        inkAtomsStory.Call("getSoundAssetName", out var soundName, soundKey);

        var duration = audioPlayer.Play(soundName);

        yield return new WaitForSeconds(duration);

        inkAtomsStory["duration"] = duration;
    }
}
