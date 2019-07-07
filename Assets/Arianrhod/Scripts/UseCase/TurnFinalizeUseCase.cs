using System;
using System.Linq;
using Arianrhod.Model;
using UniRx;
using Zenject;

namespace Arianrhod.UseCase
{
    public interface ITurnEnd
    {
        void TurnEnd();
    }
    
    public class TurnFinalizeUseCase : IInitializable, ITurnEnd,IDisposable
    {
        private readonly IPhaseRegister _phaseRegister = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly IResidueCharacters _residueCharacters = default;
        private readonly IResidueEnemies _residueEnemies = default;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public TurnFinalizeUseCase(
            IPhaseRegister phaseRegister,
            IPhaseProvider phaseProvider,
            IResidueCharacters residueCharacters,
            IResidueEnemies residueEnemies
            )
        {
            _phaseRegister = phaseRegister;
            _phaseProvider = phaseProvider;
            _residueCharacters = residueCharacters;
            _residueEnemies = residueEnemies;
        }
        
        public void Initialize()
        {
            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.End)
                .Subscribe(_ =>
                {
                    if (!_residueCharacters.Characters().Any())
                    {
                        _phaseRegister.GameOver();
                    }
                    else if (!_residueEnemies.Enemies().Any())
                    {
                        _phaseRegister.StageClear();
                    }
                    else
                    {
                        _phaseRegister.NextTurn();
                    }
                }).AddTo(_disposable);
        }

        public void TurnEnd()
        {
            _phaseRegister.NextTurn();
        }

        public void Dispose()
        {
            _disposable.Dispose();   
        }
    }
}