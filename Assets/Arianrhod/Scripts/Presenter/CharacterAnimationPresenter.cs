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

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public CharacterAnimationPresenter(
            ICharacterView characterView,
            IPhaseProvider phaseProvider,
            ICurrentSkillModel currentSkill,
            ITurnCharacterProvider turnCharacter
            )
        {
            _characterView = characterView;
            _phaseProvider = phaseProvider;
            _currentSkill = currentSkill;
            _turnCharacter = turnCharacter;
        }

        public void Initialize()
        {
            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.Damage)
                .Where(_ => _characterView.GetEntity() == _turnCharacter.OnTurnCharacterChanged().Value.CharacterEntity)
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
            ;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}