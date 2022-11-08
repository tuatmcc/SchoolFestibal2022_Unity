using System.Collections;
using UnityEngine;

namespace RaceGame.Race.Sound
{
    public class BGMManager : MonoBehaviour
    {
        private RaceManager _raceManager;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _bgmClip;
        [SerializeField] private AudioClip _startSeClip;
        private bool _startedBGM = false;
        private bool _fadeOut = false;
        //private bool _localPlayerFinished = false;
        void Start()
        {
            _raceManager = GetComponent<RaceManager>();
        }

        void Update()
        {
            if( _raceManager.RaceState == RaceState.Racing && !_startedBGM)
            {
                _audioSource.PlayOneShot(_startSeClip);
                _audioSource.volume = 1.0f;
                StartCoroutine(StartBGMAfterSconds(_audioSource, _bgmClip, 0.5f));
                _startedBGM = true;
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
                audioSource.volume -= 0.2f;
                yield return new WaitForSeconds(0.1f);
            }
            audioSource.Stop();
        }
        private IEnumerator StartBGMAfterSconds(AudioSource audioSource, AudioClip audioClip, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            audioSource.PlayOneShot(audioClip);
        }
    }
}
