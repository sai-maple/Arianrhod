using System;
using Arianrhod.Model;
using Arianrhod.UseCase;
using Arianrhod.View.Game;
using UniRx;
using Zenject;

namespace Arianrhod.Presenter
{
    public class CharacterAnimationPresenter : IInitializable,IDisposable
    {
        private readonly ICharacterView _characterView = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly ICurrentSkillModel _currentSkill = default;
        private readonly ITurnCharacterProvider _turnCharacter = default;
        private readonly IDamagePhaseFinalizer _damagePhaseFinalizer = default;
        private readonly IResidueCharacterRegister _residueCharacter = default;
        private readonly IResidueEnemyRegister _enemyRegister = default;

        private Character _character = default;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public CharacterAnimationPresenter(
            ICharacterView characterView,
            IPhaseProvider phaseProvider,
            ICurrentSkillModel currentSkill,
            ITurnCharacterProvider turnCharacter,
            IDamagePhaseFinalizer damagePhaseFinalizer,
            IResidueCharacterRegister residueCharacter,
            IResidueEnemyRegister enemyRegister 
            )
        {
            _characterView = characterView;
            _phaseProvider = phaseProvider;
            _currentSkill = currentSkill;
            _turnCharacter = turnCharacter;
            _damagePhaseFinalizer = damagePhaseFinalizer;
            _residueCharacter = residueCharacter;
            _enemyRegister = enemyRegister;
        }

        public void Initialize()
        {
            var entity = _characterView.GetEntity();
            _character = new Character(entity.Id, entity, entity.SkillEntities,entity.Owner);
            if (entity.Owner == Owner.Player)
            {
                _residueCharacter.AddCharacter(_character);
            }
            else
            {
                _enemyRegister.AddEnemy(_character);
            }

            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.Damage)
                .Where(_ => _character == _turnCharacter.OnTurnCharacterChanged().Value)
                .Subscribe(_ =>
                {
                    _characterView.OnAnimation(_currentSkill.OnSkillChanged().Value.AnimationState);
                    _characterView.OnAnimationEnded()
                        .Take(1)
                        .Subscribe(animation =>
                        {
                            _damagePhaseFinalizer.EmitDamage();
                        });
                }).AddTo(_disposable);
            _character.OnDirectionChanged()
                .Subscribe(_characterView.SetRotation).AddTo(_disposable);
            _character.Position()
                .Subscribe(_characterView.SetPosition)
                .AddTo(_disposable);
            _character.OnHpChanged()
                .Where(hp => hp <= 0)
                .Subscribe(_ => _characterView.OnDead())
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}