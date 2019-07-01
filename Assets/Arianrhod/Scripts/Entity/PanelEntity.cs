namespace Arianrhod.Entity
{
    public class PanelEntity
    {
        public int x;
        public int y;
        public PanelState panelState;
        public DiceType diceType;

        public PanelEntity(int x, int y)
        {
            this.x = x;
            this.y = y;
            diceType = EnumCommon.Random<DiceType>();
            panelState = PanelState.Dice;
        }
    }
}