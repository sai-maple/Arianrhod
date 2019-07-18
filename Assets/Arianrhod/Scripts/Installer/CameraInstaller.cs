using Arianrhod.Presenter;
using Zenject;

namespace Arianrhod.Installer
{
    public class CameraInstaller : MonoInstaller<CameraInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CameraPresenter>()
                .AsCached();
        }
    }
}