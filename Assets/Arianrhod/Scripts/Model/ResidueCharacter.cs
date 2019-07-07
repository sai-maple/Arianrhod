using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Zenject;

namespace Arianrhod.Model
{
    public interface IResidueCharacters
    {
        IEnumerable<Character> Characters();
        Character GetCharacter(int id);
        void Reset();
        void Initialize();

    }

    public interface IResidueCharacterRegister
    {
        void AddCharacter(Character character);
    }
    
    public class ResidueCharacter : IResidueCharacterRegister
    {
        private readonly List<Character> _characters = default;
        public IEnumerable<Character> Characters() => _characters;

        private IDisposable _disposable = default;


        public void Initialize()
        {
            var stream = _characters.Select(c => c.OnHpChanged()).Merge();
            _disposable = stream
                .Where(hp => hp <= 0)
                .Buffer(stream.Throttle(TimeSpan.FromMilliseconds(100)))
                .Subscribe(_ => RemoveCharacter());
        }

        public void Reset()
        {
            _characters.Clear();
            _disposable.Dispose();
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