using System;
using System.Collections.Generic;
using System.Linq;
using Arianrhod.Entity;
using Arianrhod.Model;
using JetBrains.Annotations;
using UniRx;
using Zenject;

namespace Arianrhod.UseCase
{

    public interface IPanelSelector
    {
        bool Invasive(PanelEntity target);
        Character GetCharacter(PanelEntity target);
    }
    
    public class StageUseCase : IPanelSelector, IInitializable , IDisposable
    {
        private readonly ResidueCharacter _residueCharacter = default;
        private readonly ResidueEnemy _residueEnemy = default;
        private readonly IStageModel _stageModel = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly ITargetRegister _targetRegister = default;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public StageUseCase()
        {

        }

        public void Initialize()
        {
            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == Phase.Damage)
                .Subscribe(_ => _stageModel.TargetReset())
                .AddTo(_disposable);
        }

        private void Invaded(Character character, PanelEntity target)
        {
            _stageModel.Invaded(character,target);
        }

        private void Escaped(PanelEntity target)
        {
            _stageModel.Escaped(target);
        }
        
        // 移動可能判定
        public bool Invasive(PanelEntity target)
        {
            return _stageModel.Invasive(target);
        }

        // 選択パネルのキャラ獲得　or null
        [CanBeNull]
        public Character GetCharacter(PanelEntity target)
        {
            var panel = _stageModel.GetPanel(target);
            if (panel.GetCharacterId() == -1)
            {
                return null;
            }
            var character = _residueCharacter.GetCharacter(panel.GetCharacterId());
            return character ?? _residueEnemy.GetCharacter(panel.GetCharacterId());
        }

        // 攻撃範囲の敵キャラクター取得
        public void Target(Character attacker, int skillIndex)
        {
            _stageModel.TargetReset();

            var targets = _stageModel.TargetCharacterIds(attacker, skillIndex)
                .Select(id => attacker.Owner == Owner.CPU
                    ? _residueCharacter.GetCharacter(id)
                    : _residueEnemy.GetCharacter(id))
                .Where(character => character != null)
                .ToList();

            _targetRegister.SetTargets(targets);
        }

        // キャラクターの移動委処理
        public void MoveCharacter(Character character, IEnumerable<PanelEntity> movePath)
        {
            var panelEntities = movePath.ToList();

            if(!panelEntities.Any()) return;
            
            foreach (var panel in panelEntities)
            {
                Invaded(character, panel);
                Escaped(panel);
            }

            Invaded(character, panelEntities.Last());
            _stageModel.ModeEnd(panelEntities.Last());
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}