using System;
using System.Collections.Generic;
using System.Linq;
using Arianrhod.Model;
using Arianrhod.View.Game;
using UniRx;
using UniRx.Async;
using Zenject;

namespace Arianrhod.UseCase
{
    public class StageInitializeUseCase : IInitializable ,IDisposable
    {
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly IPhaseRegister _phaseRegister = default;
        private readonly IResidueCharacters _residueCharacters = default;
        private readonly IResidueEnemies _residueEnemies = default;
        private readonly ICharacterBufferInitializer _bufferInitializer = default;
        private readonly ILoadCharacter _loadCharacter = default;
        private readonly ILoadStage _loadStage = default;
        private readonly IDiceFactory _diceFactory = default;
        private readonly ICharacterFactory _characterFactory = default;
        private readonly IStageInitializer _stageInitializer = default;
        
        private readonly List<DiceStageView> _dice = new List<DiceStageView>();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public void Initialize()
        {
            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.StageInitialize)
                .Subscribe(_ => CreateStage())
                .AddTo(_disposable);
        }

        private async void CreateStage()
        {
            foreach (var dice in _dice)
            {
                dice.Dispose();
            }
            
            if (!_residueCharacters.Characters().Any())
            {
                foreach (var character in _loadCharacter.LoadCharacters())
                {
                    _characterFactory.Create(character);
                }
                await UniTask.WaitWhile(() => _residueCharacters.Characters().Any());

                _residueCharacters.Initialize();
            }
            foreach (var character in _loadCharacter.LoadEnemies())
            {
                _characterFactory.Create(character);
            }

            await UniTask.WaitWhile(() => _residueEnemies.Enemies().Any());
            
            _bufferInitializer.UpdateCharacters(_residueCharacters.Characters().Concat(_residueEnemies.Enemies()));

            var stage = _stageInitializer.NextStage(_loadStage.LoadStage(), _residueCharacters.Characters().ToList(),
                _residueEnemies.Enemies().ToList());

            foreach (var panel in stage)
            {
                _dice.Add(_diceFactory.Create(panel.GetEntity()));
            }
            
            _phaseRegister.NextTurn();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}