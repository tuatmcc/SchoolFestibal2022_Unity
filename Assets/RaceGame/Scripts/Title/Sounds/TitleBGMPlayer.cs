using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaceGame.Title.Sound
{
    public class TitleBGMPlayer : MonoBehaviour
    {
        [SerializeField] AudioSource m_AudioSource;
        [SerializeField] AudioClip m_AudioClip;
        private void Start()
        {
            //m_AudioSource = FindObjectOfType<AudioSource>();
        }
        public void PlayTitleBGM()
        {
            // if (m_AudioSource != null)
            // {
            //     m_AudioSource.PlayOneShot(m_AudioClip);
            // }
        }
    }
}
