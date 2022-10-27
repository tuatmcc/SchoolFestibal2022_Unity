using Cinemachine;
using RaceGame.Race.Interface;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.Misc
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachineController : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;
        
        [Inject] private IRaceManager _raceManager;
        
        private void Start()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            var localPlayer = _raceManager.LocalPlayer;
            if (localPlayer == null) return;

            var localPlayerTransform = localPlayer.transform;

            _virtualCamera.Follow = localPlayerTransform;
            _virtualCamera.LookAt = localPlayerTransform;
        }
    }
}
