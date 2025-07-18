﻿namespace RaceGame.Core.Interface
{
    public interface IGameSetting
    {
        public bool StartFromTitle { get; set; }
        public PlayType PlayType { get; set; }
        public long LocalPlayerID { get; set; }
    }
}