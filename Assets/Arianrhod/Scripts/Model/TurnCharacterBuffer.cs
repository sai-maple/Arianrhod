using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Arianrhod.Model
{
    public interface ICharacterBufferInitializer
    {
        void UpdateCharacters(IEnumerable<Character> characters);
    }

    public interface ITurnCharacterProvider
    {
        IReadOnlyReactiveProperty<Character> OnTurnCharacterChanged();
    }

    public interface INextTurn
    {
        void OnNextTurn();
    }

    public class TurnCharacterBuffer : INextTurn, ICharacterBufferInitializer, ITurnCharacterProvider, IDisposable
    {
        private readonly Queue<Character> _turnCharacters = default;

        private readonly ReactiveProperty<Character> _turnCharacter = default;
        public IReadOnlyReactiveProperty<Character> OnTurnCharacterChanged() => _turnCharacter;

        public TurnCharacterBuffer()
        {
            _turnCharacters = new Queue<Character>();
            _turnCharacter = new ReactiveProperty<Character>();
        }

        public void UpdateCharacters(IEnumerable<Character> characters)
        {
            _turnCharacters.Clear();
            foreach (var character in _turnCharacters.OrderBy(character => character.CharacterEntity.Dexterity))
            {
                _turnCharacters.Enqueue(character);
            }
        }

        public void OnNextTurn()
        {
            var character = _turnCharacters.Dequeue();
            while (true)
            {
                if (character.OnHpChanged().Value <= 0)
                {
                    character = _turnCharacters.Dequeue();
                }
                else
                {
                    break;
                }
            }
            _turnCharacters.Enqueue(character);
            _turnCharacter.Value = character;
        }

        public void Dispose()
        {
            _turnCharacter.Dispose();
        }
    }
}