using System;
using Arianrhod.Model;
using Arianrhod.View.Ui;
using UniRx;
using Zenject;

namespace Arianrhod.Presenter
{
    public class TitleScreenPresenter : IInitializable, IDisposable
    {
        private readonly TitleScreen _screen = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly IPhaseRegister _phaseRegister = default;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public TitleScreenPresenter(
            TitleScreen screen,
            IPhaseProvider phaseProvider,
            IPhaseRegister phaseRegister)
        {
            _screen = screen;
            _phaseProvider = phaseProvider;
            _phaseRegister = phaseRegister;
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
        }

        private void SetEvent()
        {
            _screen.OnStart()
                .Subscribe(_ =>
                {
                    _phaseRegister.StartGame();
                    _screen.Close();
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}