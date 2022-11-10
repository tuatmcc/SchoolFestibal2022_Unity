using UnityEngine;

namespace RaceGame.Scripts.Race.UI.Parts
{
    public class CountDownCube : MonoBehaviour
    {
        [SerializeField] private Animator animator;
    
        public void Play()
        {
            animator.Play("CountDown");
        }

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
