using RaceGame.Race.Interface;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.DI
{
    /// <summary>
    /// RaceSceneのInstaller
    /// </summary>
    public class RaceSceneInstaller : MonoInstaller
    {
        [SerializeField] private RaceManager raceManager;
        
        public override void InstallBindings()
        {
            Container
                .Bind<IRaceManager>()
                .To<RaceManager>()
                .FromInstance(raceManager)
                .AsSingle()
                .NonLazy();
        }
    }
}