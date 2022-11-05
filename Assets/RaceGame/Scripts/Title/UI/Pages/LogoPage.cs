using RaceGame.Core.UI;
using UnityEngine;

namespace RaceGame.Title.UI.Pages
{
    /// <summary>
    /// TitleSceneの最初にMCCロゴを表示する
    /// </summary>
    public class LogoPage : MonoBehaviour, IPage
    {
        [SerializeField] private Animator logoAnimator;
        
        private void Update()
        {
            // アニメーションが終わり次第非アクティブ化
            if (logoAnimator.GetCurrentAnimatorStateInfo(0).IsName("Do Nothing"))
            {
                gameObject.SetActive(false);
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}