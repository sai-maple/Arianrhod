using System;
using Arianrhod.Model;
using Arianrhod.View.Game;
using UniRx;
using Zenject;

namespace Arianrhod.Presenter
{
    public class PanelPresenter : IInitializable, IDisposable
    {
        private readonly IPanelView _panelView = default;
        private readonly IStageModel _stageModel = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private PanelModel _panelModel = default;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public PanelPresenter(
            IPanelView panelView,
            IStageModel stageModel,
            IPhaseProvider phaseProvider
            )
        {
            _panelView = panelView;
            _stageModel = stageModel;
            _phaseProvider = phaseProvider;
        }

        public void Initialize()
        {
            _panelModel = _stageModel.GetPanel(_panelView.GetEntity());
            Bind();
        }

        private void Bind()
        {
            _panelModel.IsTargeted()
                .Subscribe(_panelView.OnSelect)
                .AddTo(_disposable);
        }

        private void SetEvents()
        {
            _panelView.OnPointerDown()
                .Where(_ => _phaseProvider.OnPhaseChanged().Value == Phase.Move)
                .Subscribe(entity =>
                {
                    // use case
                });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}