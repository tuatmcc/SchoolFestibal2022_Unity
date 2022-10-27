using System;
using System.Collections.Generic;
using Cinemachine;
using RaceGame.Race.Interface;
using UnityEngine;
using UnityEngine.Serialization;
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
        }


        private void Update()
        {
            AddTargets();
            var localPlayer = _raceManager.LocalPlayer;
            if (localPlayer == null) return;

            // var localPlayerTransform = localPlayer.transform;
            //
            // _virtualCamera.Follow = localPlayerTransform;
            // _virtualCamera.LookAt = localPlayerTransform;
        }

        /// <summary>
        /// CinemachineTargetGroupにプレイヤーを追加
        /// </summary>
        private void AddTargets()
        {
            // Start内だと早すぎる模様
            if (_targetGroup.m_Targets.Length < _raceManager.Players.Count)
            {
                var playerWeight = 1f;
                var localPlayerWeight = 4f;
                var playerRadius = 3f;
                var targets = new List<CinemachineTargetGroup.Target>();
                foreach (var player in _raceManager.Players)
                {
                    var target = new CinemachineTargetGroup.Target();
                    target.target = player.transform;
                    // localPlayerのウェイトだけ重くしようとしたが、判別できていない？
                    target.weight = (player == _raceManager.LocalPlayer) ? localPlayerWeight : playerWeight;
                    target.radius = playerRadius;
                    targets.Add(target);
                }

                _targetGroup.m_Targets = targets.ToArray();
                _virtualCamera.LookAt = _targetGroup.transform;
            }
        }
    }
}