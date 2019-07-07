using System;
using Arianrhod.Model;
using Arianrhod.UseCase;
using Arianrhod.View.Game;
using UniRx;
using Zenject;

namespace Arianrhod.Presenter
{
    public class MovePanelPresenter : IInitializable, IDisposable
    {
        private readonly IPanelView _panelView = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly IPanelSelector _panelSelector = default;
        private readonly IMoveLoadRegister _moveLoadRegister = default;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public MovePanelPresenter(IPanelView panelView, IPhaseProvider phaseProvider, IPanelSelector panelSelector,
            IMoveLoadRegister moveLoadRegister)
        {
            _panelView = panelView;
            _phaseProvider = phaseProvider;
            _panelSelector = panelSelector;
            _moveLoadRegister = moveLoadRegister;
        }
        
        public void Initialize()
        {
            SetEvents();
        }

        private void SetEvents()
        {
            // clear list and add panel
            _panelView.OnPointerDown()
                .Where(_ => _phaseProvider.OnPhaseChanged().Value == GamePhase.Move)
                .Where(_panelSelector.Invasive)
                .Subscribe(_moveLoadRegister.EmitFirst)
                .AddTo(_disposable);
            
            // add panel
            _panelView.OnPointerEnter()
                .Where(_ => _phaseProvider.OnPhaseChanged().Value == GamePhase.Move)
                .Where(_panelSelector.Invasive)
                .Subscribe(_moveLoadRegister.EmitPanel)
                .AddTo(_disposable);

        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}