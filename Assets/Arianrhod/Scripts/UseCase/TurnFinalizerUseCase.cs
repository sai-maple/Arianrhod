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
    
    public class TurnFinalizerUseCase : IInitializable, ITurnEnd,IDisposable
    {
        private readonly IPhaseRegister _phaseRegister = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly IResidueCharacters _residueCharacters = default;
        private readonly IResidueEnemies _residueEnemies = default;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public TurnFinalizerUseCase(
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
                .Where(phase => phase == Phase.End)
                .Subscribe(_ =>
                {
                    if (!_residueCharacters.Characters().Any())
                    {
                        // game over
                    }
                    else if (!_residueEnemies.Enemies().Any())
                    {
                        // stage clear
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