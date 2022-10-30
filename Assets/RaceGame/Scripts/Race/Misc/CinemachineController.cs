using System;
using System.Collections.Generic;
using Cinemachine;
using RaceGame.Race.Interface;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Object = System.Object;

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
            // Startだと早すぎる
            // 下のlocalPlayerが常にnullを返しているっぽいのでとりあえずそれより前に書いておく
            AddTargets();

            var localPlayer = _raceManager.LocalPlayer;
            if (localPlayer == null) return;
        }

        /// <summary>
        /// CinemachineTargetGroupにPlayersを追加する。VirtualCameraはこのTargetGroupを追う。
        /// </summary>
        private void AddTargets()
        {
            // Players を全員確実に追加するための苦肉の策. 人数が揃うまで繰り返し呼ばれてしまう
            if (_targetGroup.m_Targets.Length < _raceManager.Players.Count)
            {
                var targets = new List<CinemachineTargetGroup.Target>();
                foreach (var player in _raceManager.Players)
                {
                    var target = new CinemachineTargetGroup.Target();
                    target.target = player.transform;
                    // localPlayerのウェイトを大きくする
                    target.weight = player.IsLocalPlayer ? 4f : 1f;
                    target.radius = 3f;
                    targets.Add(target);
                }

                _targetGroup.m_Targets = targets.ToArray();
                _virtualCamera.LookAt = _targetGroup.transform;
            }
        }
    }
}