using UnityEngine;

namespace RaceGame.Race.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class ResultBGMPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip bgmSound;
        [SerializeField] private AudioClip jingleClip;
        
        private void Start()
        {
            audioSource.PlayOneShot(jingleClip);
        }
        private void Update()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(bgmSound);
            }
        }

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
}