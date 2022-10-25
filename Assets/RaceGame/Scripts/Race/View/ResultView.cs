using System.Collections.Generic;
using RaceGame.Race.Interface;
using RaceGame.Race.Network;
using TMPro;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.View
{
    /// <summary>
    /// ResultSceneのUIを動かす。RaceMangerから順位を取得する。
    /// </summary>
    public class ResultView : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] rankTexts;
        
        private IRaceManager _raceManager;
        
        [Inject]
        private void Construct(IRaceManager raceManager)
        {
            _raceManager = raceManager;
            _raceManager.OnPlayerOrderChanged += OnPlayerOrderChanged;
            _raceManager.OnRaceFinished += OnRaceFinished;
        }

        private void OnRaceFinished()
        {
            gameObject.SetActive(true);
        }

        private void OnPlayerOrderChanged(List<Player> orderedPlayers)
        {
            for (var i = 0; i < orderedPlayers.Count; i++)
            {
                rankTexts[i].text = $"{i + 1}. {orderedPlayers[i].playerName}";
                if (orderedPlayers[i].isLocalPlayer)
                {
                    rankTexts[i].color = Color.green;
                }
            }
        }
    }
}