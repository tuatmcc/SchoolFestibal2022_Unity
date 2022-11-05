using RaceGame.Race;
using UnityEngine;

namespace RaceGame.Race.Sounds
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
            if (_prevPosition == transform.position)
            {
                _prevPosition = transform.position;
                return;
            }
            _audioSource.PlayOneShot(audioClip);
            _prevPosition = transform.position;
        }
    }
}