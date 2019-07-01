namespace Arianrhod
{
    public class ScreenSignal
    {
        public ScreenState screenState { get; }

        public ScreenSignal(ScreenState state)
        {
            screenState = state;
        }
    }
}