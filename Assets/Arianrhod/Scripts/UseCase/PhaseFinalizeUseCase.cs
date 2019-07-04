using System;
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
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public PhaseFinalizeUseCase(
            IPhaseRegister phaseRegister,
            IDamageProvider damageProvider,
            ICurrentSkillModel skillModel,
            IMoveHandler moveHandler 
            )
        {
            _phaseRegister = phaseRegister;
            _damageProvider = damageProvider;
            _skillModel = skillModel;
            _moveHandler = moveHandler;
        }

        public void Initialize()
        {
            // StandBy phase to Move phase
            
            // Move phase to Attack phase
            _moveHandler.OnMoveEvent()
                .Subscribe(_ => _phaseRegister.OnNextPhase())
                .AddTo(_disposable);
            
            // Attack phase to Dice phase
            _skillModel.OnSkillChanged()
                .Subscribe(_ => _phaseRegister.OnNextPhase())
                .AddTo(_disposable);
            
            // Dice phase to Damage phase
            _damageProvider.OnDamageChanged()
                .Subscribe(_ => _phaseRegister.OnNextPhase())
                .AddTo(_disposable);
            
            // Damage phase to End phase
            
            // on Next Turn
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