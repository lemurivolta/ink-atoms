using System.Collections;

using LemuRivolta.InkAtoms;

using UnityAtoms;
using UnityAtoms.BaseAtoms;

using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create playSound external function")]
public class Test1PlaySoundExternalFunction : TaskExternalFunction, IAtomListener<GameObject>
{
    [SerializeField] private GameObjectEvent audioPlayerEvent;

    private AudioPlayer audioPlayer;

    public Test1PlaySoundExternalFunction() : base("playSound") {
    }

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

    public override IEnumerator Call(ExternalFunctionContext context)
    {
        if (audioPlayer == null) {
            Debug.LogError("no audio player in scene");
        }
        var soundName = context.Arguments[0] as string;
        var duration = audioPlayer.Play(soundName);
        yield return new WaitForSeconds(duration);
        context.Result = duration;
    }
}
