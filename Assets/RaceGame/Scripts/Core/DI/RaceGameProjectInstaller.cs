using RaceGame.Core.Interface;
using Zenject;

namespace RaceGame.Core.DI
{
    public class RaceGameProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IGameSetting>()
                .To<GameSetting>()
                .AsSingle();
        }
    }
}