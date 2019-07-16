using Arianrhod.Model;
using Zenject;

namespace Arianrhod.Installer
{
    public class GameModelInstaller : MonoInstaller<GameModelInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PhaseModel>()
                .AsCached();
            Container.BindInterfacesAndSelfTo<PanelModel>()
                .AsCached();
            Container.BindInterfacesTo<MoveLoadModel>()
                .AsCached();
            Container.BindInterfacesTo<ResidueCharacter>()
                .AsCached();
            Container.BindInterfacesTo<ResidueEnemy>()
                .AsCached();
            Container.BindInterfacesTo<StageLoadModel>()
                .AsCached();
            Container.BindInterfacesTo<StageModel>()
                .AsCached();
            Container.BindInterfacesTo<TargetCharacterModel>()
                .AsCached();
            Container.BindInterfacesTo<TurnCharacterBuffer>()
                .AsCached();
        }
    }
}