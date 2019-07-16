using Arianrhod.UseCase;
using Zenject;

namespace Arianrhod.Installer
{
    public class UseCaseInstaller : MonoInstaller<UseCaseInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<StageUseCase>()
                .AsCached();
            Container.BindInterfacesTo<StageInitializeUseCase>()
                .AsCached();
            Container.BindInterfacesTo<SkillUseCase>()
                .AsCached();
            Container.BindInterfacesTo<ResetUseCase>()
                .AsCached();
            Container.BindInterfacesTo<PhaseFinalizeUseCase>()
                .AsCached();
            Container.BindInterfacesTo<DamageCalculationUseCase>()
                .AsCached();
            Container.BindInterfacesTo<TakeDamageUseCase>()
                .AsCached();
            Container.BindInterfacesTo<TurnFinalizeUseCase>()
                .AsCached();
        }
    }
}