namespace Arianrhod.Entity
{
    public class PanelEntity
    {
        public int X { get; }
        public int Y { get; }
        public PanelState PanelState { get; private set; }
        public readonly DiceType DiceType;

        public PanelEntity(int x, int y, int state)
        {
            X = x;
            Y = y;
            DiceType = EnumCommon.Random<DiceType>();
            PanelState = (PanelState)state;
        }

        public void SetPanelState(PanelState state)
        {
            PanelState = state;
        }
    }
}