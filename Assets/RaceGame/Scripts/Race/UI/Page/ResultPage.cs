using System.Collections.Generic;
using RaceGame.Core.UI;
using RaceGame.Race.Interface;
using RaceGame.Race.Network;
using RaceGame.Race.UI.Parts;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.UI.Page
{
    /// <summary>
    /// ResultSceneのUIを動かす。RaceMangerから順位を取得する。
    /// </summary>
    public class ResultPage : MonoBehaviour, IPage
    {
        [SerializeField] private ResultRankingView[] resultRankingViews;
        
        private IRaceManager _raceManager;
        
        [Inject]
        private void Construct(IRaceManager raceManager)
        {
            _raceManager = raceManager;
            _raceManager.OnPlayerOrderChanged += OnPlayerOrderChanged;
        }

        private void OnPlayerOrderChanged(List<Player> orderedPlayers)
        {
            for (var i = 0; i < orderedPlayers.Count; i++)
            {
                if (resultRankingViews[i] == null) continue;
                
                resultRankingViews[i].SetText($"{i + 1}");
                resultRankingViews[i].SetTexture(orderedPlayers[i].TextureData?.Texture);
                if (orderedPlayers[i].isLocalPlayer)
                {
                    resultRankingViews[i].SetTextColor(Color.green);
                }
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}