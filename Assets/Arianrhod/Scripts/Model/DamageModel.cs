using System;
using UniRx;

namespace Arianrhod.Model
{
    public interface IDamageRegister
    {
        void SetDamage(int damage);
    }

    public interface IDamageProvider
    {
        IReadOnlyReactiveProperty<int> OnDamageChanged();
    }
    
    public class DamageModel : IDamageRegister,IDisposable
    {
        private readonly ReactiveProperty<int> _damage = default;
        public IReadOnlyReactiveProperty<int> OnDamageChanged() => _damage;
        
        public DamageModel()
        {
            _damage = new ReactiveProperty<int>();
        }

        public void SetDamage(int damage)
        {
            _damage.Value = damage;
        }

        public void Dispose()
        {
            _damage.Dispose();
        }
    }
}