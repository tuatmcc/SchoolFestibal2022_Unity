using System.Collections.Generic;
using RaceGame.Race.UI.Parts;
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
        [SerializeField] private Texture[] maskTextures;

        public void SetCustomTexture(Texture texture, PlayerLookType lookType)
        {
            foreach (var playerLook in playerLooks)
            {
                playerLook.gameObject.SetActive(playerLook.PlayerLookType == lookType);
                playerLook.SetCustomTexture(texture);
            }
            
            mapImage.SetTexture(texture);
            mapImage.SetMaskTexture(maskTextures[(int)lookType]);
        }
    }
}