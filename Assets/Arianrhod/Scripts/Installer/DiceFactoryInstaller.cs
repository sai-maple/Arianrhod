using Arianrhod.Entity;
using Arianrhod.View.Game;
using UnityEngine;
using Zenject;

namespace Arianrhod.Installer
{
    public class DiceFactoryInstaller : MonoInstaller<DiceFactoryInstaller>
    {
        [SerializeField] private DiceRollView _rollD3 = default;
        [SerializeField] private DiceRollView _rollD6 = default;
        [SerializeField] private DiceRollView _rollD8 = default;

        [SerializeField] private DiceStageView _stageD3 = default;
        [SerializeField] private DiceStageView _stageD6 = default;
        [SerializeField] private DiceStageView _stageD8 = default;

        public override void InstallBindings()
        {
            Container.BindFactory<int, DiceRollView, DiceRollView.D3>()
                .FromPoolableMemoryPool<int, DiceRollView, DiceD3Factory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewContextPrefab(_rollD3)
                        .UnderTransform(transform));
            Container.BindFactory<int, DiceRollView, DiceRollView.D6>()
                .FromPoolableMemoryPool<int, DiceRollView, DiceD6Factory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewContextPrefab(_rollD6)
                        .UnderTransform(transform));
            Container.BindFactory<int, DiceRollView, DiceRollView.D8>()
                .FromPoolableMemoryPool<int, DiceRollView, DiceD8Factory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewContextPrefab(_rollD8)
                        .UnderTransform(transform));

            Container.BindFactory<PanelEntity, DiceStageView, DiceStageView.D3>()
                .FromPoolableMemoryPool<PanelEntity, DiceStageView, DiceStageD3Factory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewContextPrefab(_stageD3)
                        .UnderTransform(transform));
            Container.BindFactory<PanelEntity, DiceStageView, DiceStageView.D6>()
                .FromPoolableMemoryPool<PanelEntity, DiceStageView, DiceStageD6Factory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewContextPrefab(_stageD6)
                        .UnderTransform(transform));
            Container.BindFactory<PanelEntity, DiceStageView, DiceStageView.D8>()
                .FromPoolableMemoryPool<PanelEntity, DiceStageView, DiceStageD6Factory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewContextPrefab(_stageD8)
                        .UnderTransform(transform));

            Container.BindInterfacesTo<DiceFactory>()
                .AsCached();
        }
    }

    public class DiceD3Factory : MonoPoolableMemoryPool<int, IMemoryPool, DiceRollView>
    {
    }

    public class DiceD6Factory : MonoPoolableMemoryPool<int, IMemoryPool, DiceRollView>
    {
    }

    public class DiceD8Factory : MonoPoolableMemoryPool<int, IMemoryPool, DiceRollView>
    {
    }

    public class DiceStageD3Factory : MonoPoolableMemoryPool<PanelEntity, IMemoryPool, DiceStageView>
    {
    }

    public class DiceStageD6Factory : MonoPoolableMemoryPool<PanelEntity, IMemoryPool, DiceStageView>
    {
    }

    public class DiceStageD8Factory : MonoPoolableMemoryPool<PanelEntity, IMemoryPool, DiceStageView>
    {
    }

}