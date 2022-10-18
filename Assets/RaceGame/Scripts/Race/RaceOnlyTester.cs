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
        [Inject] private ISceneManager _sceneManager;
        [Inject] private NetworkManager _networkManager;
        
        private void Start()
        {
            if (_sceneManager.StartFromTitle) return;
            
            if (!ParrelSync.ClonesManager.IsClone())
            {
                _networkManager.StartHost();
            }
            else
            {
                _networkManager.StartClient();
            }
        }
    }
}
