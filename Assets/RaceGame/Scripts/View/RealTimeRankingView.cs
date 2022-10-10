using TMPro;
using UnityEngine;
using RaceGame.Manager;

namespace RaceGame.View
{
    /// <summary>
    /// レース中にランキングUIを更新する。
    /// </summary>
    public class RealTimeRankingView : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] rankingNameTexts;

        private void Start()
        {
            var characters = RaceManager.Instance.OrderedCharacters;
            if (rankingNameTexts.Length != characters.Count)
            {
                Debug.LogError($"リアルタイムランキングの character 数({rankingNameTexts.Length})が、シーンの character 数({characters.Count})とあっていません");
                return;
            }
            
            for (var i = 0; i < characters.Count; i++)
            {
                rankingNameTexts[i].text = characters[i].playerName;
            }
        }

        private void Update()
        {
            switch (RaceManager.Instance.CurrentRaceState)
            {
                case RaceState.Racing:
                {
                    var characters = RaceManager.Instance.OrderedCharacters;
                    for (var i = 0; i < characters.Count; i++)
                    {
                        rankingNameTexts[i].text = $"{characters[i].rank}. {characters[i].playerName}";
                    }

                    break;
                }
                case RaceState.Ended:
                {
                    foreach (var textField in rankingNameTexts)
                    {
                        textField.gameObject.SetActive(false);
                    }

                    break;
                }
            }
        }
    }
}