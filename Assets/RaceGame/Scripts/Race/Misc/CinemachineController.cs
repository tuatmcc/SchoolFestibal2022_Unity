using System.Collections.Generic;
using Cinemachine;
using RaceGame.Race.Interface;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.Misc
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachineController : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup _targetGroup;
        private CinemachineVirtualCamera _virtualCamera;

        [Inject] private IRaceManager _raceManager;

        private void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _raceManager.OnRaceStandby += OnRaceStandby;
        }

        private void OnRaceStandby()
        {
            // このタイミングでカメラを最優先にする
            _virtualCamera.Priority = 100;
            
            var localPlayer = _raceManager.LocalPlayer.transform;
            _virtualCamera.LookAt = localPlayer;
            _virtualCamera.Follow = localPlayer;

        }
    }
}