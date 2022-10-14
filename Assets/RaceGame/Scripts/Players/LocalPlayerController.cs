using RaceGame.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RaceGame.Players
{
    /// <summary>
    /// ローカルプレイヤーを操作する
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class LocalPlayerController : MonoBehaviour
    {
        private Player _player;
        private CustomInputAction _customInput;

        private void Awake()
        {
            _player = GetComponent<Player>();
            
            _customInput = new CustomInputAction();
            _customInput.Enable();
        }

        private void Start()
        {
            _customInput.Player.Accelerate.started += AcceleratePlayer;
        }

        private void AcceleratePlayer(InputAction.CallbackContext context)
        {
            if (!_player.isLocalPlayer) return;
            switch (RaceManager.Instance.CurrentRaceState)
            {
                case RaceState.Racing:
                    _player.CmdAccelerate();
                    break;
            }
        }
    }
}