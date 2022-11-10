using RaceGame.Core.Interface;

namespace RaceGame.Core
{
    public class GameSetting : IGameSetting
    {
        public bool StartFromTitle { get; set; }
        public PlayType PlayType { get; set; }
        public long LocalPlayerID { get; set; }
    }
}