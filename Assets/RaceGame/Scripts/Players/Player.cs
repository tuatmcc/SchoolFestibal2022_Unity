using UnityEngine;
using UnityEngine.InputSystem;

using RaceGame.Constant;
using RaceGame.Manager;

namespace RaceGame.Players
{
    /// <summary>
    /// クリック入力を受け取り、プレイヤーを操作する。
    /// </summary>
    [RequireComponent(typeof(Character))]
    public class Player : MonoBehaviour
    {
        private CustomInputAction _customInput;
        private Character _character;

        private void Awake()
        {
            _customInput = new CustomInputAction();
            _customInput.Enable();
            
            _character = GetComponent<Character>();
        }

        private void Start()
        {
            // プレイヤー名を取得
            _character.displayName = RaceManager.Instance.PlayerName;
            _character.IsPlayer = true;
            
            _customInput.Player.Accelerate.started += AcceleratePlayer;
        }

        private void AcceleratePlayer(InputAction.CallbackContext context)
        {
            if (RaceManager.Instance.CurrentRaceState != RaceState.Started) return;
            _character.Accelerate();
        }
    }
}