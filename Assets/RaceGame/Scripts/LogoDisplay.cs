using UnityEngine;

namespace RaceGame.Scripts
{
    public class LogoDisplay : MonoBehaviour
    {
        [SerializeField] private Animator logoAnimator;

        private void Start()
        {
        }

        private void Update()
        {
            // アニメーションが終わり次第非アクティブ化
            if (logoAnimator.GetCurrentAnimatorStateInfo(0).IsName("Do Nothing"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}