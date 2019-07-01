using System.Collections.Generic;
using System.Linq;
using Arianrhod.Model;

namespace Arianrhod.UseCase
{
    public interface INextCharacter
    {
        Character OnNext();
    }

    public interface ICharacterBufferInitializer
    {
        void UpdateCharacters(IEnumerator<Character> characters);
    }

    public class TurnCharacterBuffer : INextCharacter, ICharacterBufferInitializer
    {
        private readonly Queue<Character> _turnCharacters = default;

        public TurnCharacterBuffer()
        {
            _turnCharacters = new Queue<Character>();
        }

        public void UpdateCharacters(IEnumerator<Character> characters)
        {
            _turnCharacters.Clear();
            foreach (var character in _turnCharacters.OrderBy(character => character.Dexterity))
            {
                _turnCharacters.Enqueue(character);
            }
        }

        public Character OnNext()
        {
            var character = _turnCharacters.Dequeue();
            _turnCharacters.Enqueue(character);
            return character;
        }
    }
}