using System.Collections.Generic;
using RaceGame.Race.Interface;
using RaceGame.Race.Network;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.UI.Parts
{
    /// <summary>
    /// レース中にランキングUIを更新する。
    /// </summary>
    public class RealTimeRankingViewManager : MonoBehaviour
    {
        [SerializeField] private RealTimeRankingView rankingViews;

        [Inject] private IRaceManager _raceManager;

        private void Start()
        {
            _raceManager.OnPlayerOrderChanged += OnPlayerOrderChanged;
        }

        private void OnPlayerOrderChanged(List<Player> orderedPlayers)
        {
            if (_raceManager.LocalPlayer == null) return;
            rankingViews.SetRank(_raceManager.LocalPlayer.rank);
            rankingViews.SetNamePlateTexture(_raceManager.LocalPlayer.TextureData?.Texture);
        }
    }
}