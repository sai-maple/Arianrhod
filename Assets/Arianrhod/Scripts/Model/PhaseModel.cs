using System;
using UniRx;

namespace Arianrhod.Model
{
    public interface IPhaseProvider
    {
        IObservable<Phase> OnPhaseChanged();
    }

    public interface IPhaseRegister
    {
        void NextTurn();
        void SkipMove();
        void SkipAttack();
    }

    public class PhaseModel : IPhaseProvider, IPhaseRegister, IDisposable
    {
        private readonly ReactiveProperty<Phase> _phase = default;
        public IObservable<Phase> OnPhaseChanged() => _phase;

        public void NextTurn()
        {
            _phase.Value = Phase.Standby;
        }

        public void SkipMove()
        {
            _phase.Value = Phase.Attack;
        }

        public void SkipAttack()
        {
            _phase.Value = Phase.End;
        }

        public void Dispose()
        {
            _phase.Dispose();
        }
    }
}