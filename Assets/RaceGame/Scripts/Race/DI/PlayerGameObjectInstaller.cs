using RaceGame.Race.Interface;
using RaceGame.Race.Network;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.DI
{
    public class PlayerGameObjectInstaller : MonoInstaller
    {
        [SerializeField] private Player raceManager;
        
        public override void InstallBindings()
        {
            Container
                .Bind<IPlayer>()
                .To<Player>()
                .FromInstance(raceManager)
                .AsSingle();
        }
    }
}