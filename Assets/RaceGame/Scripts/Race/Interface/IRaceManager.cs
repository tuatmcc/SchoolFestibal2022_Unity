using System;
using System.Collections.Generic;
using RaceGame.Race.Network;

namespace RaceGame.Race.Interface
{
    /// <summary>
    /// レースを管理するためのインターフェース
    /// </summary>
    public interface IRaceManager
    {
        public event Action OnRaceStandby;
        public event Action OnRaceStart;
        public event Action OnRaceFinish;
        public event Action OnCountDownStart;
        public event Action<int> OnCountDownTimerChanged;
        public event Action<List<Player>> OnPlayerOrderChanged;
        
        public RaceState RaceState { get; }
        
        public Player[] Players { get; }
        public Player LocalPlayer { get; }
        
        public void AddPlayer(Player player);
    }
}