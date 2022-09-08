using System;
using TMPro;
using UnityEngine;

namespace RaceGame.Scripts
{
    public class RealTimeRankingUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] rankingNameTexts;


        private void Start()
        {
            var characters = RaceManager.Instance.Characters;
            if (rankingNameTexts.Length != characters.Count)
            {
                Debug.LogError("リアルタイムランキングの character 数があっていません");
                return;
            }
            
            for (int i = 0; i < characters.Count; i++)
            {
                rankingNameTexts[i].text = "" + characters[i].displayName;
            }
        }

        private void Update()
        {
            if (RaceManager.Instance.CurrentRaceState == RaceStates.Started)
            {
                var characters = RaceManager.Instance.Characters;
                for (int i = 0; i < characters.Count; i++)
                {
                    rankingNameTexts[i].text = characters[i].rank + ". " + characters[i].displayName;
                }
            }
            else if (RaceManager.Instance.CurrentRaceState == RaceStates.Ended)
            {
                foreach (var textField in rankingNameTexts)
                {
                    textField.gameObject.SetActive(false);
                }
            }
        }
    }
}