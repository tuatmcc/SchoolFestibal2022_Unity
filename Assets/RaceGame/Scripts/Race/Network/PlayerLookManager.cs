using System.Collections.Generic;
using RaceGame.Race.View;
using UnityEngine;

namespace RaceGame.Race.Network
{
    /// <summary>
    /// プレイヤーの見た目を管理する
    /// </summary>
    public class PlayerLookManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerLook> playerLooks;
        [SerializeField] private MapImage mapImage;

        public void ChangeLookType(PlayerLookType lookType)
        {
            foreach (var playerLook in playerLooks)
            {
                playerLook.gameObject.SetActive(playerLook.PlayerLookType == lookType);
            }
        }

        public void SetCustomTexture(Texture texture)
        {
            foreach (var playerLook in playerLooks)
            {
                playerLook.SetCustomTexture(texture);
            }
            
            mapImage.SetTexture(texture);
        }
    }
}