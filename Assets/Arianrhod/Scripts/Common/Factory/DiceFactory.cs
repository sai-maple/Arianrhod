using System;
using System.Linq;
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
        private readonly DiceRollView.D3 _diceRollD3 = default;
        private readonly DiceRollView.D6 _diceRollD6 = default;
        private readonly DiceRollView.D8 _diceRollD8 = default;

        private readonly DiceStageView.D3 _diceStageD3 = default;
        private readonly DiceStageView.D6 _diceStageD6 = default;
        private readonly DiceStageView.D8 _diceStageD8 = default;


        public DiceFactory(
            DiceRollView.D3 diceRollD3,
            DiceRollView.D6 diceRollD6,
            DiceRollView.D8 diceRollD8,
            DiceStageView.D3 diceStageD3,
            DiceStageView.D6 diceStageD6,
            DiceStageView.D8 diceStageD8)
        {
            _diceRollD3 = diceRollD3;
            _diceRollD6 = diceRollD6;
            _diceRollD8 = diceRollD8;
            _diceStageD3 = diceStageD3;
            _diceStageD6 = diceStageD6;
            _diceStageD8 = diceStageD8;
        }

        public DiceRollView Create(DiceType diceType, int index)
        {
            switch (diceType)
            {
                case DiceType.D3:
                    return _diceRollD3.Create(index);
                case DiceType.D6:
                    return _diceRollD6.Create(index);
                case DiceType.D8:
                    return _diceRollD8.Create(index);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DiceStageView Create(PanelEntity entity)
        {
            var random = new Random();
            var result = Enum.GetValues(typeof(DiceType))
                .Cast<DiceType>()
                .OrderBy(c => random.Next())
                .FirstOrDefault();
            switch (result)
            {
                case DiceType.D3:
                    return _diceStageD3.Create(entity);
                case DiceType.D6:
                    return _diceStageD6.Create(entity);
                case DiceType.D8:
                    return _diceStageD8.Create(entity);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}