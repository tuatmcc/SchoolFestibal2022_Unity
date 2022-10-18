using System.Collections.Generic;
using RaceGame.Race.Interface;
using RaceGame.Race.Network;
using TMPro;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.View
{
    /// <summary>
    /// レース中にランキングUIを更新する。
    /// </summary>
    public class RealTimeRankingView : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] rankingNameTexts;

        [Inject] private IRaceManager _raceManager;

        private void Start()
        {
            _raceManager.OnRaceFinished += OnRaceFinished;
            _raceManager.OnPlayerOrderChanged += OnPlayerOrderChanged;
        }

        private void OnPlayerOrderChanged(List<Player> orderedPlayers)
        {
            for (var i = 0; i < orderedPlayers.Count; i++)
            {
                rankingNameTexts[i].text = $"{orderedPlayers[i].rank}. {orderedPlayers[i].playerName}";
            }
        }

        private void OnRaceFinished()
        {
            foreach (var textField in rankingNameTexts)
            {
                textField.gameObject.SetActive(false);
            }
        }
    }
}