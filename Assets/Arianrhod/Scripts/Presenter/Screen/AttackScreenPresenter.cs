using System;
using System.Linq;
using Arianrhod.Model;
using Arianrhod.UseCase;
using Arianrhod.View.Ui;
using UniRx;
using Zenject;

namespace Arianrhod.Presenter
{
    public class AttackScreenPresenter: IInitializable, IDisposable
    {
        private readonly AttackScreen _screen = default;
        private readonly IPhaseProvider _phaseProvider = default;
        private readonly IPhaseRegister _phaseRegister = default;
        private readonly ITurnCharacterProvider _turnCharacter = default;
        private readonly ITargetUseCase _targetUseCase = default;
        private readonly ITargetSubmitter _targetSubmitter = default;

        private int _index = 0;

        private IDisposable _attacker = default;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public AttackScreenPresenter(
            AttackScreen screen,
            IPhaseProvider phaseProvider,
            IPhaseRegister phaseRegister,
            ITurnCharacterProvider turnCharacter ,
            ITargetUseCase targetUseCase,
            ITargetSubmitter targetSubmitter)
        {
            _screen = screen;
            _phaseProvider = phaseProvider;
            _phaseRegister = phaseRegister;
            _turnCharacter = turnCharacter;
            _targetUseCase = targetUseCase;
            _targetSubmitter = targetSubmitter;

        }

        public void Initialize()
        {
            Bind();
            SetEvent();
        }

        private void Bind()
        {
            // attack phase になった時の画面表示
            _phaseProvider.OnPhaseChanged()
                .Where(phase => phase == GamePhase.Attack)
                .Subscribe(_ =>
                {
                    _index = 1;
                    _targetUseCase.Target(_index);
                    _screen.Open();
                })
                .AddTo(_disposable);

            // ターン変更時のキャラクター更新
            _turnCharacter.OnTurnCharacterChanged()
                .Subscribe(character =>
                {
                    _attacker.Dispose();
                    _attacker = character.OnDirectionChanged()
                        .Where(_ => _phaseProvider.OnPhaseChanged().Value == GamePhase.Attack)
                        .Subscribe(_ =>  _targetUseCase.Target(_index));
                }).AddTo(_disposable);

            //　ターゲット変更時のSubmitボタンの有効可否
            _targetSubmitter.TargetOnChanged()
                .Subscribe(targets => _screen.IsEmitable(targets.Count != 0))
                .AddTo(_disposable);
        }

        private void SetEvent()
        {
            _screen.OnSkillSelected()
                .Subscribe(index =>
                {
                    _index = index;
                    _targetUseCase.Target(_index);
                    _screen.Initialize(_turnCharacter.OnTurnCharacterChanged().Value.SkillEntities().ToList());
                })
                .AddTo(_disposable);

            _screen.OnDirectionChanged()
                .Subscribe(_turnCharacter.OnTurnCharacterChanged().Value.SetDirection)
                .AddTo(_disposable);

            _screen.OnSubmit()
                .Subscribe(_ =>
                {
                    _screen.Close();
                    _targetSubmitter.OnSubmit();
                })
                .AddTo(_disposable);

            _screen.OnSkip()
                .Subscribe(_ =>
                {
                    _targetUseCase.SkipAttack();
                    _phaseRegister.SkipAttack();
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