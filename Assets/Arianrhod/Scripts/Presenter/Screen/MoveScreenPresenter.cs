using System;
using Arianrhod.Model;
using Arianrhod.View.Ui;
using UniRx;
using Zenject;

namespace Arianrhod.Presenter
{
    public class MoveScreenPresenter : IInitializable, IDisposable
    {
        private readonly MoveScreen _screen = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly IPhaseRegister _phaseRegister = default;
        private readonly IMoveLoadRegister _moveLoadRegister = default;
        private readonly ITurnCharacterProvider _turnCharacter = default;
        
        private readonly CompositeDisposable _player = new CompositeDisposable();
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public MoveScreenPresenter(
            MoveScreen screen,
            IPhaseProvider phaseProvider,
            IPhaseRegister phaseRegister,
            IMoveLoadRegister moveLoadRegister ,
            ITurnCharacterProvider turnCharacter)
        {
            _screen = screen;
            _phaseProvider = phaseProvider;
            _phaseRegister = phaseRegister;
            _moveLoadRegister = moveLoadRegister;
            _turnCharacter = turnCharacter;
        }

        public void Initialize()
        {
            Bind();
            SetEvent();
        }

        private void Bind()
        {
            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.OutGame)
                .Subscribe(_ => _screen.Open())
                .AddTo(_disposable);
            _moveLoadRegister.OnLoadChanged()
                .ObserveCountChanged()
                .Subscribe(count => _screen.IsMovable(count != 0));

            _turnCharacter.OnTurnCharacterChanged()
                .Subscribe(CharacterEvents).AddTo(_disposable);
        }

        private void CharacterEvents(Character character)
        {
            _player.Clear();
            character.OnHpChanged()
                .Subscribe(hp => _screen.CharacterHp(hp, character.MaxHp))
                .AddTo(_player);

            character.OnD3Changed()
                .Subscribe(_screen.CharacterD3Num)
                .AddTo(_player);
            character.OnD6Changed()
                .Subscribe(_screen.CharacterD6Num)
                .AddTo(_player);
            character.OnD8Changed()
                .Subscribe(_screen.CharacterD8Num)
                .AddTo(_player);
        }

        private void SetEvent()
        {
            _screen.OnMoveSubmit()
                .Subscribe(_ =>
                {
                    _moveLoadRegister.OnSubmit();
                    _screen.Close();
                })
                .AddTo(_disposable);
            
            _screen.OnMoveSkip()
                .Subscribe(_ =>
                {
                    _phaseRegister.SkipMove();
                    _screen.Close();
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _player.Dispose();
            _disposable.Dispose();
        }
    }
}