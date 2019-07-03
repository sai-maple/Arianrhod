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
        
        private readonly Vector3[] _path =
        {
            new Vector3(0f, 1f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(2.5f, 0.5f, 0f),
            new Vector3(3f, 0f, 0f),
        };

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
    }
}