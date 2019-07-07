using System;
using System.Collections.Generic;
using Arianrhod.Model;
using UniRx;
using Zenject;

namespace Arianrhod.UseCase
{
    public interface IDicePhaseFinalizer
    {
        void SetDamageNum(int damage);
    }

    public class DamageCalculationUseCase : IInitializable, IDicePhaseFinalizer, IDisposable
    {
        private readonly IDamageRegister _damageRegister = default;
        private readonly ITargetProvider _targetProvider = default;

        private List<Character> _targets = default;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public DamageCalculationUseCase(
            IDamageRegister damageRegister,
            ITargetProvider targetProvider)
        {
            _damageRegister = damageRegister;
            _targetProvider = targetProvider;
        }

        public void Initialize()
        {
            _targetProvider.OnTargetCharacters()
                .Where(targets => targets.Count != 0)
                .Subscribe(targets => _targets = targets)
                .AddTo(_disposable);
        }

        public void SetDamageNum(int damage)
        {
            // 向きとか入れたい

            _damageRegister.SetDamage(damage);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}