using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaceGame.Race.Sounds
{
    public class BGMManager : MonoBehaviour
    {
        private RaceManager _raceManager;
        [SerializeField] private GameObject _target;
        [SerializeField] private AudioClip _bgmClip;
        private AudioSource _audioSource;
        private bool _fadeOut = false;
        void Start()
        {
            _raceManager = GetComponent<RaceManager>();
            _audioSource = _target.GetComponent<AudioSource>();
        }

        void Update()
        {
            if( _raceManager.RaceState == RaceState.Racing && !_audioSource.isPlaying )
            {
                _audioSource.PlayOneShot(_bgmClip);
            }
            if( _raceManager.RaceState == RaceState.Finished && !_fadeOut )
            {
                StartCoroutine(FadeOut(_audioSource));
                _fadeOut = true;
            }
        }
        private IEnumerator FadeOut(AudioSource audioSource)
        {
            while(audioSource.volume > 0)
            {
                audioSource.volume -= 0.02f;
                yield return new WaitForSeconds(0.1f);
            }
            audioSource.Stop();
        }
    }
}
