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
    public class StageInitializeUseCase : IInitializable, IDisposable
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
        private readonly PanelView.Factory _panelFactory = default;

        private readonly List<DiceStageView> _dice = new List<DiceStageView>();
        private readonly List<PanelView> _panel = new List<PanelView>();
        
        private readonly List<CharacterView> _characters = new List<CharacterView>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private int _id = 0;

        public StageInitializeUseCase(
            IPhaseProvider phaseProvider,
            IPhaseRegister phaseRegister,
            IResidueCharacters residueCharacters,
            IResidueEnemies residueEnemies,
            ICharacterBufferInitializer bufferInitializer,
            ILoadCharacter loadCharacter,
            ILoadStage loadStage,
            IDiceFactory diceFactory,
            ICharacterFactory characterFactory,
            IStageInitializer stageInitializer,
            PanelView.Factory panelFactory)
        {
            _phaseProvider = phaseProvider;
            _phaseRegister = phaseRegister;
            _residueCharacters = residueCharacters;
            _residueEnemies = residueEnemies;
            _bufferInitializer = bufferInitializer;
            _loadCharacter = loadCharacter;
            _loadStage = loadStage;
            _diceFactory = diceFactory;
            _characterFactory = characterFactory;
            _stageInitializer = stageInitializer;
            _panelFactory = panelFactory;
        }

        public void Initialize()
        {
            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.StageInitialize)
                .Subscribe(_ => CreateStage())
                .AddTo(_disposable);
        }

        private void Reset()
        {
            foreach (var dice in _dice)
            {
                dice.Dispose();
            }
            _dice.Clear();
            
            foreach (var panel in _panel)
            {
                panel.Dispose();
            }
            _panel.Clear();
            
            foreach (var character in _characters)
            {
                character.Dispose();
            }
            _characters.Clear();
        }

        private async void CreateStage()
        {
            Reset();

            foreach (var character in _loadCharacter.LoadCharacters())
            {
                _characters.Add(_characterFactory.Create(_id, character,Owner.Player));
                _id++;
            }

            await UniTask.WaitWhile(() => _residueCharacters.Characters().Any());

            _residueCharacters.Initialize();

            foreach (var character in _loadCharacter.LoadEnemies())
            {
                _characters.Add(_characterFactory.Create(_id, character,Owner.CPU));
                _id++;
            }

            await UniTask.WaitWhile(() => _residueEnemies.Enemies().Any());
            _residueEnemies.Initialize();

            _bufferInitializer.UpdateCharacters(_residueCharacters.Characters().Concat(_residueEnemies.Enemies()));

            var stage = _stageInitializer.NextStage(_loadStage.LoadStage(), _residueCharacters.Characters().ToList(),
                _residueEnemies.Enemies().ToList());

            foreach (var panel in stage)
            {
                _dice.Add(_diceFactory.Create(panel.GetEntity()));
                var entity = panel.GetEntity();
                if(entity.PanelState == PanelState.NoEntry) continue;
                _panel.Add(_panelFactory.Create(entity));
            }

            _phaseRegister.NextTurn();
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}