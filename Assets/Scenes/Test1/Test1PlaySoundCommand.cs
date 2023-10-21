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

    private AudioPlayer audioPlayer;

    public Test1PlaySoundCommand() : base("playSound") { }

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

    public override IEnumerator Invoke(IDictionary<string, Parameter> parameters)
    {
        var soundName = GetParameter(parameters, "soundName");

        if (audioPlayer == null)
        {
            Debug.LogError("no audio player in scene");
        }
        var duration = audioPlayer.Play(soundName);
        yield return new WaitForSeconds(duration);
    }
}
