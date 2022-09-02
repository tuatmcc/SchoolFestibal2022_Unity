using UnityEngine;
using UnityEngine.InputSystem;

namespace RaceGame.Scripts
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
        }

        private void Start()
        {
            _character = GetComponent<Character>();

            // Set Player Name
            _character.displayName = RaceManager.Instance.PlayerDisplayName;
            _character.IsPlayer = true;

            
            _customInput.Player.Accelerate.started += AcceleratePlayer;
        }

        private void AcceleratePlayer(InputAction.CallbackContext context)
        {
            if (RaceManager.Instance.CurrentRaceState != RaceManager.RaceStates.Started) return;
            _character.Accelerate();
        }
    }
}