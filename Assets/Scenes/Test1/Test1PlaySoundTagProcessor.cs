using System.Collections;
using System.Collections.Generic;

using LemuRivolta.InkAtoms;

using UnityAtoms;
using UnityAtoms.BaseAtoms;

using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create playSound tag processor")]
public class Test1PlaySoundTagProcessor : TagProcessor, IAtomListener<GameObject>
{
    [SerializeField] private GameObjectEvent audioPlayerEvent;
    [SerializeField] private InkAtomsStoryEvent inkAtomsStoryInitializedEvent;

    private AudioPlayer audioPlayer;
    private InkAtomsStory inkAtomsStory;

    public Test1PlaySoundTagProcessor() : base("play-sound") { }

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

    public override IEnumerator Process(IReadOnlyList<string> parameters, StoryStep storyStep)
    {
        if (audioPlayer == null)
        {
            Debug.LogError("no audio player in scene");
        }
        var soundKey = parameters[0];

        var soundNameOperation = inkAtomsStory.CallAndWait("getSoundAssetName", soundKey);
        yield return soundNameOperation;

        var duration = audioPlayer.Play(soundNameOperation.TextOutput);
        yield return new WaitForSeconds(duration);
    }
}
