using UnityEngine;

namespace RaceGame.Players
{
    public class PlayerInfo
    {
        public string DisplayName;
        public int PlayerId;
        public Texture CustomTexture;

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