using Mirror;
using UnityEngine;

namespace RaceGame.Race.Sound
{
    public class SePlayer : MonoBehaviour
    {
        private Vector3 _prevPosition;
        
        [SerializeField] private AudioClip audioClip;
        private AudioSource _audioSource;
        private RaceManager _raceManager;

        private void Start()
        {
            _prevPosition = transform.position;
            _audioSource = GetComponent<AudioSource>();
            _raceManager = FindObjectOfType<RaceManager>();
        }
        public void PlayFootStep()
        {
            if (_raceManager.RaceState != RaceState.Racing) return;
            if (!NetworkClient.isConnected) return;
            if (_prevPosition == transform.position)
            {
                _prevPosition = transform.position;
                return;
            }
            // ���ɂȂ��Ă���Ƃ��͊m���Ŗ炷
            if (_audioSource.isPlaying) return;
            _audioSource.PlayOneShot(audioClip);
            _prevPosition = transform.position;
        }
    }
}