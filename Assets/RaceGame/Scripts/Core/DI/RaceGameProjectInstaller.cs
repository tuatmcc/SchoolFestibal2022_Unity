using Mirror;
using RaceGame.Core.Interface;
using UnityEngine;
using Zenject;

namespace RaceGame.Core.DI
{
    /// <summary>
    /// Project全体のProjectInstaller
    /// </summary>
    public class RaceGameProjectInstaller : MonoInstaller
    {
        [SerializeField] private NetworkManager networkManager;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ISceneManager>()
                .To<RaceGameSceneManager>()
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<NetworkManager>()
                .To<NetworkManager>()
                .FromInstance(networkManager)
                .AsSingle()
                .NonLazy();
        }
    }
}