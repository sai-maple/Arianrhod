using Arianrhod.Model;
using Arianrhod.Presenter;
using Zenject;

namespace Arianrhod.Installer
{
    public class EnemyInstaller : Installer<EnemyInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CpuCharacterPresenter>()
                .AsCached();
            Container.BindInterfacesAndSelfTo<Character>()
                .AsCached();

        }
    }
}