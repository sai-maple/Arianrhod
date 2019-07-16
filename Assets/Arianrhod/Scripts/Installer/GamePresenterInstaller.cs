using Arianrhod.Presenter;
using Zenject;

namespace Arianrhod.Installer
{
    public class GamePresenterInstaller : MonoInstaller<GamePresenterInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<StandByInitializer>()
                .AsCached();
            Container.BindInterfacesTo<DiceRollPresenter>()
                .AsCached();
        }
    }
}