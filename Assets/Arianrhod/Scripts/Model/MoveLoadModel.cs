using System;
using System.Collections.Generic;
using Arianrhod.Entity;
using UniRx;

namespace Arianrhod.Model
{
    public interface IMoveLoadProvider
    {
        IReadOnlyReactiveCollection<PanelEntity> OnLoadChanged();
        IObservable<IEnumerable<PanelEntity>> OnLoadSubmit();
    }

    public interface IMoveLoadRegister
    {
        void EmitFirst(PanelEntity entity);
        void EmitPanel(PanelEntity entity);
        void OnSubmit();
        IReadOnlyReactiveCollection<PanelEntity> OnLoadChanged();
    }

    public class MoveLoadModel : IMoveLoadProvider, IMoveLoadRegister, IDisposable
    {
        private readonly ReactiveCollection<PanelEntity> _load = default;
        public IReadOnlyReactiveCollection<PanelEntity> OnLoadChanged() => _load;
        private readonly Subject<IEnumerable<PanelEntity>> _loadSubject = default;
        public IObservable<IEnumerable<PanelEntity>> OnLoadSubmit() => _loadSubject.Publish().RefCount();

        public MoveLoadModel()
        {
            _load = new ReactiveCollection<PanelEntity>();
        }

        public void EmitFirst(PanelEntity entity)
        {
            _load.Clear();
            _load.Add(entity);
        }

        public void EmitPanel(PanelEntity entity)
        {
            if (_load.Contains(entity))
            {
                _load.Remove(entity);
            }
            else
            {
                _load.Add(entity);
            }
        }

        public void OnSubmit()
        {
            _loadSubject.OnNext(_load);
        }

        public void Dispose()
        {
            _load.Dispose();
            _loadSubject.OnCompleted();
            _loadSubject.Dispose();
        }
    }
}