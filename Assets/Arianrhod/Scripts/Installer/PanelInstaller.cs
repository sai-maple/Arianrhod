using Arianrhod.Presenter;
using Zenject;

namespace Arianrhod.Installer
{
    public class PanelInstaller : Installer<PanelInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PanelPresenter>()
                .AsCached();
            Container.BindInterfacesTo<MovePanelPresenter>()
                .AsCached();
        }
    }
}