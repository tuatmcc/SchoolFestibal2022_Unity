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
            rankingViews.SetText(_raceManager.LocalPlayer.rank.ToString());
            rankingViews.SetTexture(_raceManager.LocalPlayer.TextureData?.Texture);
            switch (_raceManager.LocalPlayer.rank)
            {
                case 1:
                    rankingViews.SetTextColor(new Color32(211, 169, 008, 255));
                    break;
                case 2:
                    rankingViews.SetTextColor(new Color32(187, 189, 192, 255));
                    break;
                case 3:
                    rankingViews.SetTextColor(new Color32(189, 163, 102, 255));
                    break;
                default:
                    rankingViews.SetTextColor(Color.black);
                    break;
            }
        }
    }
}