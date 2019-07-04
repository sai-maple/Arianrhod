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
    
    public class TargetCharacterModel :ITargetRegister, ITargetProvider, IDisposable
    {
        private readonly Subject<List<Character>> _targets = default;
        public IObservable<List<Character>> OnTargetCharacters() => _targets;

        public TargetCharacterModel()
        {
            _targets = new Subject<List<Character>>();
        }

        public void SetTargets(List<Character> targets)
        {
            _targets.OnNext(targets);
        }

        public void Dispose()
        {
            _targets.Dispose();
        }
    }
}