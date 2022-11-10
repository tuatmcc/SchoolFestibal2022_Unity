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
            if (_prevPosition == transform.position)
            {
                _prevPosition = transform.position;
                return;
            }
            // Šù‚É‚È‚Á‚Ä‚¢‚é‚Æ‚«‚ÍŠm—¦‚Å–Â‚ç‚·
            if (_audioSource.isPlaying) return;
            _audioSource.PlayOneShot(audioClip);
            _prevPosition = transform.position;
        }
    }
}