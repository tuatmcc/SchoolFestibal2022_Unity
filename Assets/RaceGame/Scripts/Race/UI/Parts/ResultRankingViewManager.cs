using System.Collections.Generic;
using System.Linq;
using RaceGame.Race.Interface;
using RaceGame.Race.Network;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.UI.Parts
{
    /// <summary>
    /// レース中にランキングUIを更新する。
    /// </summary>
    public class ResultRankingViewManager : MonoBehaviour
    {
        [SerializeField] private ResultRankingView[] rankingViews;

        private IRaceManager _raceManager;

        [Inject]
        private void Const(IRaceManager raceManager)
        {
            _raceManager = raceManager;
            _raceManager.OnRaceFinish += OnRaceFinish;
        }

        private void OnRaceFinish()
        {
            if (_raceManager.LocalPlayer == null) return;

            var orderedPlayers = _raceManager.Players.OrderBy(x => x.rank).ToList();
            for (var i = 0; i < orderedPlayers.Count; i++)
            {
                var player = orderedPlayers[i];
                var rankingView = rankingViews[i];
                rankingView.SetTexture(player.TextureData.Texture);
                var time = player.GoalTime;
                var timeString = $"{time.Minutes:00}:{time.Seconds:00}.{time.Milliseconds:00}";
                rankingView.SetTimeText(timeString);
                rankingView.SetClickCountText(player.ClickCount.ToString());
            }
        }
    }
}