using RaceGame.Race.Interface;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RaceGame.Race.Network
{
    /// <summary>
    /// ローカルプレイヤーを操作する
    /// </summary>
    public class LocalPlayerController : MonoBehaviour
    {
        [SerializeReference] private IPlayer _player;
        
        [Inject] private IRaceManager _raceManager;
        
        private CustomInputAction _customInput;

        private void Awake()
        {
            _customInput = new CustomInputAction();
            _customInput.Enable();
        }

        private void Start()
        {
            _customInput.Player.Accelerate.started += AcceleratePlayer;
        }

        private void AcceleratePlayer(InputAction.CallbackContext context)
        {
            if (!_player.IsLocalPlayer) return;
            
            switch (_raceManager.RaceState)
            {
                case RaceState.Racing:
                    _player.CmdAccelerate();
                    break;
            }
        }
    }
}