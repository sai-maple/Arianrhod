using System;
using Arianrhod.Model;
using Arianrhod.View.Ui;
using UniRx;
using Zenject;

namespace Arianrhod.Presenter
{
    public class GameOverScreenPresenter : IInitializable, IDisposable
    {
        private readonly GameOverScreen _screen = default;
        private readonly IPhaseRegister _phaseRegister = default;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public GameOverScreenPresenter(
            GameOverScreen screen,
            IPhaseRegister phaseRegister
            )
        {
            _screen = screen;
            _phaseRegister = phaseRegister;
        }

        public void Initialize()
        {
            _screen.OnReturn()
                .Subscribe(_ =>
                {
                    _screen.Close();
                    _phaseRegister.ReturnTitle();
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}