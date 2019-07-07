using Arianrhod.Entity;
using Arianrhod.View.Game;

namespace Arianrhod
{
    public interface ICharacterFactory
    {
        CharacterView Create(int id,CharacterEntity entity);
    }
    
    public class CharacterFactory : ICharacterFactory
    {
        private readonly CharacterView.Factory _factory = default;

        public CharacterView Create(int id,CharacterEntity entity)
        {
            return _factory.Create(id,entity);
        }
    }
}