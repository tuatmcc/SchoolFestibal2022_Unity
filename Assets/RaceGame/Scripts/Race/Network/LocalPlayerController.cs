using Mirror;
using RaceGame.Race.Interface;
using RaceGame.Race.Sound;
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
        private IPlayer _player;
        
        [Inject] private IRaceManager _raceManager;
        
        private CustomInputAction _customInput;
        private SePlayer _sePlayer;

        // TODO : 雑な処理ですみません…
        private SePlayer SePlayer
        {
            get
            {
                if (_sePlayer == null)
                {
                    _sePlayer = GetComponent<SePlayer>();
                }

                return _sePlayer;
            }
        }

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
        }

        private void AcceleratePlayer(InputAction.CallbackContext context)
        {
            if (!_player.IsLocalPlayer) return;

            // TODO : 雑な処理ですみません…
            if (!NetworkClient.isConnected) return;
            // TODO : 雑な処理ですみません…
            if (this == null)
            {
                Debug.LogWarning("悲しい");
                return;
            }

            switch (_raceManager.RaceState)
            {
                case RaceState.Racing:
                    _player.CmdAccelerate();
                    SePlayer.PlayFootStep();
                    break;
            }
        }
    }
}