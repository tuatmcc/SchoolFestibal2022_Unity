using RaceGame.Race.Interface;
using RaceGame.Race.Sounds;
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
        private SePlayer _sePlayer;

        private void Start()
        {
            _customInput = new CustomInputAction();
            _customInput.Enable();
            _customInput.Player.Accelerate.started += AcceleratePlayer;
            
            // 急ぎで雑なやり方
            // 本来であればFactoryPattern等で対応する
            if (_player == null)
            {
                _player = GetComponent<Player>();
            }

            if (_raceManager == null)
            {
                _raceManager = FindObjectOfType<RaceManager>();
            }

            _sePlayer = GetComponent<SePlayer>();
        }

        private void AcceleratePlayer(InputAction.CallbackContext context)
        {
            if (!_player.IsLocalPlayer) return;
            
            switch (_raceManager.RaceState)
            {
                case RaceState.Racing:
                    _player.CmdAccelerate();
                    _sePlayer.PlayFootStep();
                    break;
            }
        }
    }
}