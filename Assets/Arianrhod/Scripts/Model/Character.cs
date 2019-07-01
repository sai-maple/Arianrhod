using Arianrhod.Entity;

namespace Arianrhod.Model
{
    public class Character
    {
        private readonly CharacterEntity _characterEntity = default;
        private DiceEntity _dice { get; }

        public int Dexterity => _characterEntity.Dexterity;
        
        public void AddDice(DiceType diceType)
        {
            
        }
    }
}