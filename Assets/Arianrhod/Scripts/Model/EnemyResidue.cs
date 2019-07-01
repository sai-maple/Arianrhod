using System.Collections.Generic;
using System.Linq;
using UniRx;
using Zenject;

namespace Arianrhod.Model
{
    public class EnemyResidue 
    {
        private readonly List<Character> _enemies = default;
        public IEnumerable<Character> Characters() => _enemies;

        public void NextStage()
        {
            _enemies.Select(c => c.OnHpChanged()).Merge()
                .Where(hp => hp <= 0)
                .Subscribe(_ => RemoveCharacter());
        }

        private void RemoveCharacter()
        {
            foreach (var character in _enemies)
            {
                if (character.OnHpChanged().Value <= 0)
                {
                    _enemies.Remove(character);
                }
            }

            if (_enemies.Count == 0)
            {
                // stage clear
            }
        }
    }
}