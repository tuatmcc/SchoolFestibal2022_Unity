using System.Collections.Generic;
using RaceGame.Race.Interface;
using RaceGame.Race.Network;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.View
{
    /// <summary>
    /// レース中にランキングUIを更新する。
    /// </summary>
    public class RealTimeRankingViewManager : MonoBehaviour
    {
        [SerializeField] private RealTimeRankingView[] rankingViews;

        [Inject] private IRaceManager _raceManager;

        private void Start()
        {
            _raceManager.OnRaceFinished += OnRaceFinished;
            _raceManager.OnPlayerOrderChanged += OnPlayerOrderChanged;
        }

        private void OnPlayerOrderChanged(List<Player> orderedPlayers)
        {
            Debug.Log($"{nameof(OnPlayerOrderChanged)}");
            for (var i = 0; i < orderedPlayers.Count; i++)
            {
                rankingViews[i].SetText(orderedPlayers[i].rank.ToString());
                rankingViews[i].SetTexture(orderedPlayers[i].TextureData?.Texture);
            }
        }

        private void OnRaceFinished()
        {
            foreach (var textField in rankingViews)
            {
                textField.gameObject.SetActive(false);
            }
        }
    }
}