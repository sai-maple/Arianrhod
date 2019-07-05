using System;
using System.Collections.Generic;
using Arianrhod.Entity;
using UniRx;

namespace Arianrhod.Model
{
    public class CharacterMoveLoadModel : IDisposable
    {
        private readonly ReactiveCollection<PanelEntity> _load = default;
        public IReadOnlyReactiveCollection<PanelEntity> OnLoadChanged => _load;
        private readonly Subject<List<PanelEntity>> _loadSubject = default;
        public IObservable<List<PanelEntity>> OnLoadSubmit() => _loadSubject.Publish().RefCount();

        public CharacterMoveLoadModel()
        {
            _load = new ReactiveCollection<PanelEntity>();
        }

        public void EmitFirst()
        {
            
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
        
        public void Dispose()
        {
            _load.Dispose();
            _loadSubject.OnCompleted();
            _loadSubject.Dispose();
        }
    }
}