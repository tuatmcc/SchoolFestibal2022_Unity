using RaceGame.Constant;
using RaceGame.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RaceGame.Players
{
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
            // Set Player Name
            _character.displayName = PlayerInfo.Instance.DisplayName;
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