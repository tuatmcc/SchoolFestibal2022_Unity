using System;
using RaceGame.Race.Interface;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.UI.Parts
{
    public class RealTimeTimerView : MonoBehaviour
    {
        [SerializeField] private Text timerText;
        [Inject] private IRaceManager _raceManager;

        private void Start()
        {
            _raceManager.OnRaceStart += () => timerText.gameObject.SetActive(true);
            _raceManager.OnRaceFinish += () => timerText.gameObject.SetActive(false);
        }

        // RealTimeRankingViewManagerを参考に
        // Player.csに新たにCurrentTimeと_currentTimeを作成
        private void Update()
        {
            if (_raceManager.RaceState != RaceState.Racing) return;
            if (_raceManager.LocalPlayer == null) return;

            var localPlayer = _raceManager.LocalPlayer;
            var time = localPlayer.CurrentTime;

            if (_raceManager.RaceState != RaceState.Racing) return;
            var timeString = $"{time.Minutes:00}:{time.Seconds:00}.{time.Milliseconds:000}";
            timerText.text = timeString;
        }
    }
}