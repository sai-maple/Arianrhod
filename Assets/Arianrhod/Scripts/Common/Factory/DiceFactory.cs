using Arianrhod.Entity;
using Arianrhod.View.Game;

namespace Arianrhod
{
    public interface IDiceFactory
    {
        DiceRollView Create(DiceType diceType, int index);
        
        DiceStageView Create(PanelEntity entity);
    }
    
    public class DiceFactory : IDiceFactory
    {
        private readonly DiceRollView.Factory _diceRollFactory = default;
        private readonly DiceStageView.Factory _diceStageFactory = default;
        
        public DiceFactory(
            DiceRollView.Factory diceRollFactory
            )
        {
            _diceRollFactory = diceRollFactory;
        }

        public DiceRollView Create(DiceType diceType, int index)
        {
            return _diceRollFactory.Create(index);
        }
        
        public DiceStageView Create(PanelEntity entity)
        {
            return _diceStageFactory.Create(entity);
        }
    }
}