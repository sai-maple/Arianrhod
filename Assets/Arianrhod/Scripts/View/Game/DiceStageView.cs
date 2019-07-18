using System;
using Arianrhod.Entity;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Arianrhod.View.Game
{
    // ステージ上のダイス
    public class DiceStageView: MonoBehaviour, IPoolable<PanelEntity, IMemoryPool>, IDisposable
    {
        private IMemoryPool _pool = default;
        [SerializeField] private ObservableEventTrigger _trigger = default;

        private CompositeDisposable _disposable = default;

        private async void DeSpawnAnimation()
        {
            await transform.DOScale(Vector3.zero, 1);
            OnDespawned();
        }

        public void OnDespawned()
        {
            if (_pool == null) return;
            Dispose();
        }

        public void OnSpawned(PanelEntity panel, IMemoryPool pool)
        {
            _pool = pool;
            _disposable = new CompositeDisposable();
            transform.localPosition = new Vector3(panel.X, 1f, panel.Y);

            _trigger.OnTriggerEnterAsObservable()
                .Where(col => col.gameObject.CompareTag("Character"))
                .Subscribe(_ => DeSpawnAnimation())
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _pool.Despawn(this);
        }
        
        public class D3 : PlaceholderFactory<PanelEntity, DiceStageView>{}
        public class D6 : PlaceholderFactory<PanelEntity, DiceStageView>{}
        public class D8 : PlaceholderFactory<PanelEntity, DiceStageView>{}

    }
}