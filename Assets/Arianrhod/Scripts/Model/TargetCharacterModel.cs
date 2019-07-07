using System;
using System.Collections.Generic;
using UniRx;

namespace Arianrhod.Model
{
    public interface ITargetRegister
    {
        void SetTargets(List<Character> targets);
    }

    public interface ITargetProvider
    {
        IObservable<List<Character>> OnTargetCharacters();
    }

    public interface ITargetSubmitter
    {
        IReadOnlyReactiveProperty<List<Character>> TargetOnChanged();
        void OnSubmit();
    }

    public class TargetCharacterModel : ITargetRegister, ITargetProvider, ITargetSubmitter, IDisposable
    {
        private readonly ReactiveProperty<List<Character>> _targetsList = default;
        public IReadOnlyReactiveProperty<List<Character>> TargetOnChanged() => _targetsList;
        private readonly Subject<List<Character>> _targets = default;
        public IObservable<List<Character>> OnTargetCharacters() => _targets;

        public TargetCharacterModel()
        {
            _targetsList = new ReactiveProperty<List<Character>>();
            _targets = new Subject<List<Character>>();
        }

        public void SetTargets(List<Character> targets)
        {
            _targetsList.Value = targets;
        }

        public void OnSubmit()
        {
            _targets.OnNext(_targetsList.Value);
        }

        public void Dispose()
        {
            _targets.OnCompleted();
            _targets.Dispose();
        }
    }
}