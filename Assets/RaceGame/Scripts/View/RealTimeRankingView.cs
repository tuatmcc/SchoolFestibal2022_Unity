using System;
using TMPro;
using UnityEngine;
using RaceGame.Manager;

using RaceGame.Constant;

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
            
            for (int i = 0; i < characters.Count; i++)
            {
                rankingNameTexts[i].text = characters[i].displayName;
            }
        }

        private void Update()
        {
            if (RaceManager.Instance.CurrentRaceState == RaceState.Started)
            {
                var characters = RaceManager.Instance.OrderedCharacters;
                for (int i = 0; i < characters.Count; i++)
                {
                    rankingNameTexts[i].text = $"{characters[i].rank}. {characters[i].displayName}";
                }
            }
            else if (RaceManager.Instance.CurrentRaceState == RaceState.Ended)
            {
                foreach (var textField in rankingNameTexts)
                {
                    textField.gameObject.SetActive(false);
                }
            }
        }
    }
}