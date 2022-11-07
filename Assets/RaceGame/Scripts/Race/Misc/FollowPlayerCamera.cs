using Cinemachine.PostFX;
using RaceGame.Race.Interface;
using UnityEngine;
using Zenject;

public class FollowPlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachinePostProcessing cinemachinePostProcessing;

    [Inject] private IRaceManager _raceManager;
    
    private void Update()
    {
        var localPlayer = _raceManager?.LocalPlayer;
        if (localPlayer == null) return;
        
        cinemachinePostProcessing.m_FocusTarget = localPlayer.transform;
    }

    private void Reset()
    {
        cinemachinePostProcessing = GetComponent<CinemachinePostProcessing>();
    }
}
