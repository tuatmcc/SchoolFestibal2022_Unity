using Mirror;
using RaceGame.Core.Interface;
using UnityEngine;
using Zenject;

namespace RaceGame.Race
{
    /// <summary>
    /// 直接RaceSceneを開いたときにネットワークの準備をするためのもの
    /// </summary>
    public class RaceOnlyTester : MonoBehaviour
    {
        [Inject] private IGameSetting _gameSetting;

        private void Start()
        {
            if (_gameSetting.StartFromTitle) return;
            
            if (!ParrelSync.ClonesManager.IsClone())
            {
                NetworkManager.singleton.StartHost();
            }
            else
            {
                NetworkManager.singleton.StartClient();
            }
        }
    }
}
