using System;
using Arianrhod.Model;
using Arianrhod.View.Game;
using UniRx;
using Zenject;

namespace Arianrhod.Presenter
{
    public class ArrowPresenter : IInitializable , IDisposable
    {
        private readonly IMoveArrow _moveArrow = default;
        private readonly IMoveLoadProvider _loadProvider = default;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public void Initialize()
        {
            _loadProvider.OnLoadChanged()
                .ObserveCountChanged()
                .Subscribe(_ => _moveArrow.SetArrow(_loadProvider.OnLoadChanged()))
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}