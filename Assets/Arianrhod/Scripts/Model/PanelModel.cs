using System;
using Arianrhod.Entity;
using UniRx;

namespace Arianrhod.Model
{
    public class PanelModel : IDisposable
    {
        public PanelModel()
        {
            _isTarget = new ReactiveProperty<bool>(false);
        }
        
        private readonly PanelEntity _entity = default;
        private int _characterId = -1;

        private readonly ReactiveProperty<bool> _isTarget = default;

        public IObservable<bool> IsTargeted() => _isTarget;

        public int GetCharacterId()
        {
            return _entity.PanelState == PanelState.Character ? _characterId : -1;
        }

        public void Invaded(Character character)
        {
            if (_entity.PanelState == PanelState.Dice)
            {
                character.AddDice(_entity.DiceType);
            }

            _characterId = character.Id;
            character.SetPosition(_entity);
            _entity.SetPanelState(PanelState.Character);
        }

        public void Escaped()
        {
            _characterId = -1;
            _entity.SetPanelState(PanelState.Empty);
        }

        public bool Invasive()
        {
            return _entity.PanelState != PanelState.Character;
        }

        public void Target(bool isTarget)
        {
            _isTarget.Value = isTarget;
        }

        public void Dispose()
        {
            _isTarget.Dispose();
        }
    }
}