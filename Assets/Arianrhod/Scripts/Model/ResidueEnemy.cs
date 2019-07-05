using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Arianrhod.Model
{
    public interface IResidueEnemies
    {
        IEnumerable<Character> Enemies();
    }
    
    public class ResidueEnemy : IResidueEnemies
    {
        private readonly List<Character> _enemies = default;
        public IEnumerable<Character> Enemies() => _enemies;

        private IDisposable _disposable = default;

        public void NextStage()
        {
            _disposable.Dispose();
            var stream = _enemies.Select(c => c.OnHpChanged()).Merge();
            _disposable = stream
                .Where(hp => hp <= 0)
                .Buffer(stream.Throttle(TimeSpan.FromMilliseconds(100)))
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