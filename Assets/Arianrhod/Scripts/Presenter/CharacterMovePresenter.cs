using Arianrhod.Model;
using Arianrhod.UseCase;
using Arianrhod.View.Game;
using UniRx;
using Zenject;

namespace Arianrhod.Presenter
{
    public class CharacterMovePresenter : IInitializable
    {
        private readonly IPanelView _panelView = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly IPanelSelector _panelSelector = default;
        
        public CharacterMovePresenter(IPanelView panelView, IPhaseProvider phaseProvider, IPanelSelector panelSelector)
        {
            _panelView = panelView;
            _phaseProvider = phaseProvider;
            _panelSelector = panelSelector;
        }
        
        public void Initialize()
        {
            SetEvents();
        }

        private void SetEvents()
        {
            // clear list and add panel
            _panelView.OnPointerDown()
                .Where(_ => _phaseProvider.OnPhaseChanged().Value == Phase.Move)
                .Where(_panelSelector.Invasive)
                .Subscribe(_ => { });
            
            // add panel
            _panelView.OnPointerEnter()
                .Where(_ => _phaseProvider.OnPhaseChanged().Value == Phase.Move)
                .Where(_panelSelector.Invasive)
                .Subscribe(_ => { });

            // view submit ui
            _panelView.OnPointerDown()
                .Where(_ => _phaseProvider.OnPhaseChanged().Value == Phase.Move)
                .Where(_panelSelector.Invasive)
                .Subscribe(_ => { });
        }
    }
}