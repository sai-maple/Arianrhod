using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Arianrhod.Model
{
    public class EnemyResidue 
    {
        private readonly List<Character> _enemies = default;
        public IEnumerable<Character> Enemies() => _enemies;

        private IDisposable _disposable = default;

        public void NextStage()
        {
            _disposable.Dispose();
            _disposable = _enemies.Select(c => c.OnHpChanged()).Merge()
                .Where(hp => hp <= 0)
                .Subscribe(_ => RemoveCharacter());
        }

        public void AddEnemy(Character enemy)
        {
            _enemies.Add(enemy);
        }
        
        public Character GetCharacter(int id)
        {
            return _enemies.FirstOrDefault(character => character.Id == id);
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
                _enemies.Clear();
            }
        }
    }
}