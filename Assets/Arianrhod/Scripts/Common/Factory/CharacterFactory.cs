using Arianrhod.Entity;
using Arianrhod.View.Game;

namespace Arianrhod
{
    public interface ICharacterFactory
    {
        CharacterView Create(CharacterEntity entity);
    }
    
    public class CharacterFactory
    {
        private readonly CharacterView.Factory _factory = default;

        public CharacterView Create(CharacterEntity entity)
        {
            return _factory.Create(entity);
        }
    }
}