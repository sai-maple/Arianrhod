using System;
using Arianrhod.Entity;
using UniRx;
using UnityEngine;
using UniRx.Triggers;
using Zenject;

namespace Arianrhod.View.Game
{
    public interface IPanelView
    {
        IObservable<PanelEntity> OnPointerEnter();
        IObservable<PanelEntity> OnPointerDown();
        IObservable<PanelEntity> OnPointerUp();
        void OnSelect(bool isSelect);
        PanelEntity GetEntity();
        void OnDespawned();
    }
    
    public class PanelView : MonoBehaviour, IPoolable<PanelEntity, IMemoryPool>,
        IDisposable , IPanelView
    {
        private IMemoryPool _pool = default;
        private PanelEntity _panelEntity = default;
        
        [SerializeField] private ObservableEventTrigger _trigger = default;
        [SerializeField] private GameObject _selectView = default;

        public IObservable<PanelEntity> OnPointerEnter() =>
            _trigger.OnPointerEnterAsObservable()
                .Select(_ => _panelEntity);
        
        public IObservable<PanelEntity> OnPointerDown() =>
            _trigger.OnPointerDownAsObservable()
                .Select(_ => _panelEntity);
        
        public IObservable<PanelEntity> OnPointerUp() =>
            _trigger.OnPointerUpAsObservable()
                .Select(_ => _panelEntity);

        public PanelEntity GetEntity() => _panelEntity;

        public void OnSelect(bool isSelect)
        {
            _selectView.SetActive(isSelect);
        }

        public void OnDespawned()
        {
            if (_pool == null) return;
            Dispose();
        }

        public void OnSpawned(PanelEntity panelEntity, IMemoryPool pool)
        {
            _pool = pool;
            _panelEntity = panelEntity;
            transform.localPosition = new Vector3(panelEntity.X, panelEntity.Y);
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }
    }
}