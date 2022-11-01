using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaceGame.Race.Sounds
{
    public class PlayerSeManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] audioClips;
        private AudioSource audioSource;
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void PlayFootStep()
        {
            audioSource.PlayOneShot( audioClips[Random.Range(0, audioClips.Length)] );
        }
    }
}