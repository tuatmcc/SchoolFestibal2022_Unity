using RaceGame.Core.UI;
using UnityEngine;

namespace RaceGame.Race.UI.Page
{
    /// <summary>
    /// ResultSceneのUIを動かす。RaceMangerから順位を取得する。
    /// </summary>
    public class ResultPage : MonoBehaviour, IPage
    {
        [SerializeField] private SelectableButton retryButton;
        [SerializeField] private SelectableButton backToTitleButton;
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}