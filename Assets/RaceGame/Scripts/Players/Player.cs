using UnityEngine;
using UnityEngine.InputSystem;

namespace RaceGame.Scripts.Players
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
            _character.displayName = PlayerInfo.Instance.displayName;
            _character.IsPlayer = true;

            
            _customInput.Player.Accelerate.started += AcceleratePlayer;
        }

        private void AcceleratePlayer(InputAction.CallbackContext context)
        {
            if (RaceManager.Instance.CurrentRaceState != RaceStates.Started) return;
            _character.Accelerate();
        }
    }
}