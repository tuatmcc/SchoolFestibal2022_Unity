using UnityEngine;

namespace RaceGame.Title.View
{
    /// <summary>
    /// TitleSceneの最初にMCCロゴを表示する
    /// </summary>
    public class LogoView : MonoBehaviour
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
    }
}