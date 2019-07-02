using System.Collections.Generic;
using System.Linq;
using UniRx;
using Zenject;

namespace Arianrhod.Model
{
    public class CharacterResidue : IInitializable
    {
        private readonly List<Character> _characters = default;
        public IEnumerable<Character> Characters() => _characters;

        public void Initialize()
        {
            _characters.Select(c => c.OnHpChanged()).Merge()
                .Where(hp => hp <= 0)
                .Subscribe(_ => RemoveCharacter());
        }
        
        public void AddCharacter(Character character)
        {
            _characters.Add(character);
        }

        public Character GetCharacter(int id)
        {
            return _characters.FirstOrDefault(character => character.Id == id);
        }

        private void RemoveCharacter()
        {
            foreach (var character in _characters)
            {
                if (character.OnHpChanged().Value <= 0)
                {
                    _characters.Remove(character);
                }
            }

            if (_characters.Count == 0)
            {
                // game over
                _characters.Clear();
            }
        }
    }
}