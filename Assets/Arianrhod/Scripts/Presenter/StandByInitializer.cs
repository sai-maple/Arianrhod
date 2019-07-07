using System;
using Arianrhod.Model;
using UniRx;
using Zenject;

namespace Arianrhod.Presenter
{
    public class StandByInitializer : IInitializable , IDisposable
    {
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly INextTurn _nextTurn = default;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public StandByInitializer(IPhaseProvider phaseProvider, INextTurn nextTurn)
        {
            _phaseProvider = phaseProvider;
            _nextTurn = nextTurn;
        }

        public void Initialize()
        {
            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.Standby)
                .Subscribe(_ => _nextTurn.OnNextTurn())
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}