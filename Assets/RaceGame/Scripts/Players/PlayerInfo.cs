using Unity.VisualScripting;
using UnityEngine;

namespace RaceGame.Scripts.Players
{
    public class PlayerInfo
    {
        public string displayName;
        public int playerId;
        public Texture customTexture;

        private static PlayerInfo _instance;
        public static PlayerInfo Instance
        {
            get
            {
                _instance = _instance ?? new PlayerInfo();
                return _instance;
            }
        }
    }
}