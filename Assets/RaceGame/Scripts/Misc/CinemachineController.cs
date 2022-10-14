using System;
using Cinemachine;
using RaceGame.Manager;
using UnityEngine;

namespace RaceGame.Misc
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachineController : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;

        private void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            _virtualCamera.Follow = RaceManager.Instance.localPlayer.transform;
            _virtualCamera.LookAt = RaceManager.Instance.localPlayer.transform;
        }
    }
}
