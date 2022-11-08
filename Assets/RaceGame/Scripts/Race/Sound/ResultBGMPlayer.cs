using UnityEngine;

namespace RaceGame.Race.Sound
{
    public class ResultBGMPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip bgmSound;
        [SerializeField] private AudioClip jingleClip;
        private void Awake()
        {
            audioSource = GameObject.Find("MainCamera").GetComponent<AudioSource>();
        }
        private void Start()
        {
            audioSource.PlayOneShot(bgmSound);
        }
    }
}