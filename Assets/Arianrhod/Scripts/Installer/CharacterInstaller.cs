using Arianrhod.Model;
using Arianrhod.Presenter;
using Zenject;

namespace Arianrhod.Installer
{
    public class CharacterInstaller : Installer<CharacterInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CharacterPresenter>()
                .AsCached().NonLazy();
            Container.BindInterfacesAndSelfTo<Character>()
                .AsCached();
            Container.BindInterfacesTo<CharacterMovePresenter>()
                .AsCached().NonLazy();
        }
    }
}