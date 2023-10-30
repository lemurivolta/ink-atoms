using System.Collections;
using System.Collections.Generic;

using LemuRivolta.InkAtoms;

using UnityAtoms;
using UnityAtoms.BaseAtoms;

using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create playSound command line parser")]
public class Test1PlaySoundCommand : CommandLineParser, IAtomListener<GameObject>
{
    [SerializeField] private GameObjectEvent audioPlayerEvent;
    [SerializeField] private InkAtomsStoryEvent inkAtomsStoryInitializedEvent;

    private AudioPlayer audioPlayer;
    private InkAtomsStory inkAtomsStory;

    public Test1PlaySoundCommand() : base("playSound") { }

    private void OnEnable()
    {
        audioPlayerEvent.RegisterListener(this);
        inkAtomsStoryInitializedEvent.Register(OnInkAtomsStoryInitialized);
    }

    private void OnDisable()
    {
        audioPlayerEvent.UnregisterListener(this);
        inkAtomsStoryInitializedEvent.Unregister(OnInkAtomsStoryInitialized);
    }

    public void OnEventRaised(GameObject item)
    {
        audioPlayer = item.GetComponent<AudioPlayer>();
    }

    public void OnInkAtomsStoryInitialized(InkAtomsStory inkAtomsStory)
    {
        this.inkAtomsStory = inkAtomsStory;
    }

    public override IEnumerator Invoke(IDictionary<string, Parameter> parameters)
    {
        if (audioPlayer == null)
        {
            Debug.LogError("no audio player in scene");
            yield break;
        }

        var soundKey = GetParameter(parameters, "soundName");

        inkAtomsStory.Call("getSoundAssetName", out var soundName, out var _, soundKey);

        var duration = audioPlayer.Play(soundName);
        yield return new WaitForSeconds(duration);
    }
}
