using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Arianrhod.Model
{
    public interface ICharacterBufferInitializer
    {
        void UpdateCharacters(IEnumerator<Character> characters);
    }

    public class TurnCharacterBuffer : INextTurn, ICharacterBufferInitializer , IDisposable
    {
        private readonly Queue<Character> _turnCharacters = default;

        private readonly ReactiveProperty<Character> _turnCharacter = default;
        public IReadOnlyReactiveProperty<Character> OnTurnCharacterChanged() => _turnCharacter;

        public TurnCharacterBuffer()
        {
            _turnCharacters = new Queue<Character>();
            _turnCharacter = new ReactiveProperty<Character>();
        }

        public void UpdateCharacters(IEnumerator<Character> characters)
        {
            _turnCharacters.Clear();
            foreach (var character in _turnCharacters.OrderBy(character => character.CharacterEntity.Dexterity))
            {
                _turnCharacters.Enqueue(character);
            }
        }

        public void OnNext()
        {
            var character = _turnCharacters.Dequeue();
            _turnCharacters.Enqueue(character);
            _turnCharacter.Value = character;
        }

        public void Dispose()
        {
            _turnCharacter.Dispose();
        }
    }
}