using System.Collections;

using LemuRivolta.InkAtoms;

using UnityAtoms;
using UnityAtoms.BaseAtoms;

using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create playSound external function")]
public class Test1PlaySoundExternalFunction : CoroutineExternalFunction, IAtomListener<GameObject>
{
    [SerializeField] private GameObjectEvent audioPlayerEvent;
    [SerializeField] private InkAtomsStoryEvent inkAtomsStoryInitializedEvent;

    private AudioPlayer audioPlayer;
    private InkAtomsStory inkAtomsStory;

    public Test1PlaySoundExternalFunction() : base("playSound")
    {
    }

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

    public override IEnumerator Call(ExternalFunctionContextWithResult context)
    {
        if (audioPlayer == null)
        {
            Debug.LogError("no audio player in scene");
        }
        var soundKey = context.Arguments[0] as string;
        inkAtomsStory.Call("getSoundAssetName", out var soundName, out var _, soundKey);
        var duration = audioPlayer.Play(soundName);
        context.ReturnValue = duration;
        yield return new WaitForSeconds(duration);
    }
}
