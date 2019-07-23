using Arianrhod.Entity;
using Arianrhod.View.Game;
using UnityEngine;
using Zenject;

namespace Arianrhod.Installer
{
    public class PanelFactoryInstaller : MonoInstaller<PanelFactoryInstaller>
    {
        [SerializeField] private PanelView _panelPrefab = default;

        public override void InstallBindings()
        {
            Container.BindFactory<PanelEntity, PanelView, PanelView.Factory>()
                .FromPoolableMemoryPool<PanelEntity, PanelView, PanelFactory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewPrefabInstaller<PanelInstaller>(_panelPrefab)
                        .UnderTransform(transform));
        }
    }
    
    public class PanelFactory : MonoPoolableMemoryPool<PanelEntity, IMemoryPool, PanelView>
    {
    }
}