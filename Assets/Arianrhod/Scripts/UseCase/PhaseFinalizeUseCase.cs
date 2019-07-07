using System;
using System.Linq;
using Arianrhod.Model;
using UniRx;
using Zenject;

namespace Arianrhod.UseCase
{
    public interface IPhaseFinalizer
    {
        void SkipMove();
        void SkipAttack();
    }
    
    public class PhaseFinalizeUseCase : IInitializable, IPhaseFinalizer, IDisposable
    {
        private readonly IPhaseRegister _phaseRegister = default;
        private readonly IDamageProvider _damageProvider = default;
        private readonly ICurrentSkillModel _skillModel = default;
        private readonly IMoveHandler _moveHandler = default;
        private readonly ITurnCharacterProvider _turnCharacter = default; 
        private readonly ITargetProvider _targetProvider = default;

        private IDisposable _targets = default;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public PhaseFinalizeUseCase(
            IPhaseRegister phaseRegister,
            IDamageProvider damageProvider,
            ICurrentSkillModel skillModel,
            IMoveHandler moveHandler,
            ITurnCharacterProvider turnCharacter,
            ITargetProvider targetProvider
            )
        {
            _phaseRegister = phaseRegister;
            _damageProvider = damageProvider;
            _skillModel = skillModel;
            _moveHandler = moveHandler;
            _turnCharacter = turnCharacter;
            _targetProvider = targetProvider;
        }

        public void Initialize()
        {
            // StandBy phase to Move phase
            _turnCharacter.OnTurnCharacterChanged()
                .Subscribe(_ => _phaseRegister.OnNextPhase())
                .AddTo(_disposable);
            
            // Move phase to Attack phase
            _moveHandler.OnMoveEvent()
                .Subscribe(_ => _phaseRegister.OnNextPhase())
                .AddTo(_disposable);
            
            // Attack phase to Dice phase
            _skillModel.OnSkillSubmit()
                .Subscribe(_ => _phaseRegister.OnNextPhase())
                .AddTo(_disposable);
            
            // Dice phase to Damage phase
            _damageProvider.OnDamageChanged()
                .Subscribe(_ => _phaseRegister.OnNextPhase())
                .AddTo(_disposable);
            
            // Damage phase to End phase
            _targetProvider.OnTargetCharacters()
                .Subscribe(targets =>
                {
                    _targets.Dispose();
                    var stream = targets.Select(c => c.OnHpChanged()).Merge();
                    _targets = stream
                        .Buffer(stream.Throttle(TimeSpan.FromMilliseconds(100)))
                        .Subscribe(_ => _phaseRegister.OnNextPhase());
                })
                .AddTo(_disposable);
        }

        public void SkipMove()
        {
            _phaseRegister.SkipMove();
        }

        public void SkipAttack()
        {
            _phaseRegister.SkipAttack();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}