using Arianrhod.Presenter;
using Zenject;

namespace Arianrhod.Installer
{
    public class ArrowInstaller : MonoInstaller<ArrowInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ArrowPresenter>()
                .AsCached().NonLazy();
        }
    }
}