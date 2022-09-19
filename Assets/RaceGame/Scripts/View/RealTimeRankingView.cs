using RaceGame.Constant;
using RaceGame.Manager;
using TMPro;
using UnityEngine;

namespace RaceGame.View
{
    public class RealTimeRankingView : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] rankingNameTexts;

        private void Start()
        {
            var characters = RaceManager.Instance.OrderedCharacters;
            if (rankingNameTexts.Length != characters.Count)
            {
                Debug.LogError("リアルタイムランキングの character 数があっていません");
                return;
            }
            
            for (var i = 0; i < characters.Count; i++)
            {
                rankingNameTexts[i].text = characters[i].displayName;
            }
        }

        private void Update()
        {
            if (RaceManager.Instance.CurrentRaceState == RaceState.Started)
            {
                var characters = RaceManager.Instance.OrderedCharacters;
                for (var i = 0; i < characters.Count; i++)
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