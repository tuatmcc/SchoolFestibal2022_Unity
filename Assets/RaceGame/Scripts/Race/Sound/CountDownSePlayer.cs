using UnityEngine;

namespace RaceGame.Race.Sound
{
    public class CountDownSePlayer : MonoBehaviour
    {
        [SerializeField] AudioSource m_AudioSource;
        [SerializeField] AudioClip firstClip;
        [SerializeField] AudioClip finalClip;
        private void Start()
        {
            m_AudioSource = GameObject.Find("MainCamera").GetComponent<AudioSource>();
        }
        public void PlayFirstSE()
        {
            m_AudioSource.volume = 0.5f;
            m_AudioSource.PlayOneShot(firstClip);
        }
        public void PlayFinalSE()
        {
            m_AudioSource.volume = 0.5f;
            m_AudioSource.PlayOneShot(finalClip);
        }
    }
}