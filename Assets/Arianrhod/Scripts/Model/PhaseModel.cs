using System;
using UniRx;

namespace Arianrhod.Model
{
    public interface IPhaseProvider
    {
        IReadOnlyReactiveProperty<GamePhase> OnPhaseChanged();
    }

    public interface IPhaseRegister
    {
        void NextTurn();
        void SkipMove();
        void SkipAttack();
        void OnNextPhase();

        void GameOver();
        void StageClear();
        void StartGame();
    }

    public class PhaseModel : IPhaseProvider, IPhaseRegister, IDisposable
    {
        private readonly ReactiveProperty<GamePhase> _phase = default;
        public IReadOnlyReactiveProperty<GamePhase> OnPhaseChanged() => _phase;

        public void NextTurn()
        {
            _phase.Value = GamePhase.Standby;
        }

        public void SkipMove()
        {
            _phase.Value = GamePhase.Attack;
        }

        public void SkipAttack()
        {
            _phase.Value = GamePhase.End;
        }

        public void OnNextPhase()
        {
            _phase.Value = (GamePhase)(((int)_phase.Value + 1) % Enum.GetValues(typeof(GamePhase)).Length);
        }

        public void GameOver()
        {
            _phase.Value = GamePhase.GameOver;
        }

        public void StageClear()
        {
            _phase.Value = GamePhase.StageClear;
        }

        public void StartGame()
        {
            _phase.Value = GamePhase.StageInitialize;
        }

        public void Dispose()
        {
            _phase.Dispose();
        }
    }
}