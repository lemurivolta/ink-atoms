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

    private AudioPlayer audioPlayer;

    public Test1PlaySoundTagProcessor() : base("play-sound") { }

    private void OnEnable()
    {
        audioPlayerEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        audioPlayerEvent.UnregisterListener(this);
    }

    public void OnEventRaised(GameObject item)
    {
        audioPlayer = item.GetComponent<AudioPlayer>();
    }

    public override IEnumerator Process(IReadOnlyList<string> parameters, StoryStep storyStep)
    {
        if (audioPlayer == null)
        {
            Debug.LogError("no audio player in scene");
        }
        var duration = audioPlayer.Play(parameters[0]);
        yield return new WaitForSeconds(duration);
    }
}
