using System;
using System.Collections.Generic;
using Arianrhod.Model;
using UniRx;
using Zenject;

namespace Arianrhod.UseCase
{
    public interface IDamagePhaseFinalizer
    {
        void EmitDamage();
    }
    
    public class TakeDamageUseCase :IInitializable, IDamagePhaseFinalizer , IDisposable
    {
        private readonly IDamageProvider _damageProvider = default;
        private readonly ITargetProvider _targetProvider = default;

        private List<Character> _targets = default;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public TakeDamageUseCase(
            IDamageProvider damageProvider,
            ITargetProvider targetProvider)
        {
            _damageProvider = damageProvider;
            _targetProvider = targetProvider;
        }
        
        public void Initialize()
        {
            _targetProvider.OnTargetCharacters()
                .Where(targets => targets.Count != 0)
                .Subscribe(targets => _targets = targets)
                .AddTo(_disposable);
        }

        public void EmitDamage()
        {
            foreach (var character in _targets)
            {
                character.EmitDamage(_damageProvider.OnDamageChanged().Value);
            }
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}