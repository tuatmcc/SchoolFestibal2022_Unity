using RaceGame.Core.Interface;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.Network
{
    public class PlayerIDDebugger : MonoBehaviour
    {
        [SerializeField] private int localPlayerID;
        [Inject] private IGameSetting _gameSetting;

        private void Start()
        {
            _gameSetting.LocalPlayerID = localPlayerID;
        }
    }
}
