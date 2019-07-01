namespace Arianrhod.Entity
{
    public class PanelEntity
    {
        public int X;
        public int Y;
        public PanelState PanelState;
        public readonly DiceType DiceType;

        public PanelEntity(int x, int y)
        {
            X = x;
            Y = y;
            DiceType = EnumCommon.Random<DiceType>();
            PanelState = PanelState.Dice;
        }
    }
}