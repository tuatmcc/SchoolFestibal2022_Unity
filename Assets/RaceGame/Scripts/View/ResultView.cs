using RaceGame.Scene;
using UnityEngine;
using TMPro;

using RaceGame.Manager;

namespace RaceGame.View
{
    /// <summary>
    /// ResultSceneのUIを動かす。RaceMangerから順位を取得する。
    /// </summary>
    public class ResultView : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] rankTexts;

        private void Start()
        {
            for (var i = 0; i < RaceManager.Instance.OrderedCharacters.Count; i++)
            {
                rankTexts[i].text = $"{i + 1}. {RaceManager.Instance.OrderedCharacters[i].displayName}";
                if (RaceManager.Instance.OrderedCharacters[i].IsPlayer)
                {
                    rankTexts[i].color = Color.green;
                }
            }
        }
    }
}