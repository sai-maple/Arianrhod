using Arianrhod.View.Game;

namespace Arianrhod
{
    public interface IDiceFactory
    {
        DiceRollView Create(DiceType diceType, int index);
    }
    
    public class DiceFactory : IDiceFactory
    {
        private readonly DiceRollView.Factory _diceFactory = default;
        
        public DiceFactory(DiceRollView.Factory diceFactory)
        {
            _diceFactory = diceFactory;
        }

        public DiceRollView Create(DiceType diceType, int index)
        {
            return _diceFactory.Create(index);
        }
    }
}