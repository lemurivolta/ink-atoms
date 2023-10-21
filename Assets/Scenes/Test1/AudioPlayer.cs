using System.Collections;
using System.Collections.Generic;

using UnityAtoms.BaseAtoms;

using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private GameObjectEvent audioPlayerEvent;
        [SerializeField] private AudioSource audioSource;

        private void Start()
        {
            audioPlayerEvent.Raise(gameObject);
        }

        public float Play(string soundName)
        {
            Debug.Log("using " + soundName);
            audioSource.Play();
            return audioSource.clip.length;
        }
    }
}
