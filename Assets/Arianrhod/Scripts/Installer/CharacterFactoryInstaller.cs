using Arianrhod.Entity;
using Arianrhod.View.Game;
using UnityEngine;
using Zenject;

namespace Arianrhod.Installer
{
    public class CharacterFactoryInstaller : MonoInstaller<CharacterFactoryInstaller>
    {
        [SerializeField] private CharacterView _guanyu = default;
        [SerializeField] private CharacterView _huanggai = default;
        [SerializeField] private CharacterView _huanyueing = default;
        [SerializeField] private CharacterView _lusu = default;
        [SerializeField] private CharacterView _simayi = default;
        [SerializeField] private CharacterView _zhugeliang = default;

        public override void InstallBindings()
        {
            Container.BindFactory<int, Owner, CharacterEntity, CharacterView, CharacterView.Factory>()
                .FromPoolableMemoryPool<int, Owner, CharacterEntity, CharacterView, GuanyuFactory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewPrefabInstaller<EnemyInstaller>(_guanyu)
                        .UnderTransform(transform));
            Container.BindFactory<int, Owner, CharacterEntity, CharacterView, CharacterView.Factory>()
                .FromPoolableMemoryPool<int, Owner, CharacterEntity, CharacterView, HuanggaiFactory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewPrefabInstaller<EnemyInstaller>(_huanggai)
                        .UnderTransform(transform));
            Container.BindFactory<int, Owner, CharacterEntity, CharacterView, CharacterView.Factory>()
                .FromPoolableMemoryPool<int, Owner, CharacterEntity, CharacterView, HuanyueingFactory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewPrefabInstaller<EnemyInstaller>(_huanyueing)
                        .UnderTransform(transform));
            Container.BindFactory<int, Owner, CharacterEntity, CharacterView, CharacterView.Factory>()
                .FromPoolableMemoryPool<int, Owner, CharacterEntity, CharacterView, LusuFactory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewPrefabInstaller<CharacterInstaller>(_lusu)
                        .UnderTransform(transform));
            Container.BindFactory<int, Owner, CharacterEntity, CharacterView, CharacterView.Factory>()
                .FromPoolableMemoryPool<int, Owner, CharacterEntity, CharacterView, SimayiFactory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewPrefabInstaller<CharacterInstaller>(_simayi)
                        .UnderTransform(transform));
            Container.BindFactory<int, Owner, CharacterEntity, CharacterView, CharacterView.Factory>()
                .FromPoolableMemoryPool<int, Owner, CharacterEntity, CharacterView, ZhugeliangFactory>(poolBuilder =>
                    poolBuilder
                        .WithInitialSize(1)
                        .FromSubContainerResolve()
                        .ByNewPrefabInstaller<CharacterInstaller>(_zhugeliang)
                        .UnderTransform(transform));
        }

        public class GuanyuFactory : MonoPoolableMemoryPool<int, Owner, CharacterEntity, IMemoryPool, CharacterView>
        {
        }

        public class HuanggaiFactory : MonoPoolableMemoryPool<int, Owner, CharacterEntity, IMemoryPool, CharacterView>
        {
        }

        public class HuanyueingFactory : MonoPoolableMemoryPool<int, Owner, CharacterEntity, IMemoryPool, CharacterView>
        {
        }

        public class LusuFactory : MonoPoolableMemoryPool<int, Owner, CharacterEntity, IMemoryPool, CharacterView>
        {
        }

        public class SimayiFactory : MonoPoolableMemoryPool<int, Owner, CharacterEntity, IMemoryPool, CharacterView>
        {
        }

        public class ZhugeliangFactory : MonoPoolableMemoryPool<int, Owner, CharacterEntity, IMemoryPool, CharacterView>
        {
        }

    }
}