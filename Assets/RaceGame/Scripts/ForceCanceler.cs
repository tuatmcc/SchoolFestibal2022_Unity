using Mirror;
using RaceGame.Race.Interface;
using RaceGame.Race.UI.Page;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RaceGame
{
    public class ForceCanceler: MonoBehaviour
    {
        [Inject] private IRaceManager _raceManager; 
        private CustomInputAction _customInput;

        private void Start()
        {
            _customInput = new CustomInputAction();
            _customInput.Enable();
            _customInput.UI.ForceCancel.started += BackToTitle;
        }

        private void BackToTitle(InputAction.CallbackContext context)
        {
            if (_raceManager.LocalPlayer.isServer)
            {
                NetworkManager.singleton.StopHost();
            }
            else
            {
                NetworkManager.singleton.StopClient();
            }
        }
    }
}