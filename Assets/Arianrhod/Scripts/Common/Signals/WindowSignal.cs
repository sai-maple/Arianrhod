namespace Arianrhod
{
    public class WindowSignal
    {
        public WindowState windowState { get; }

        public WindowSignal(WindowState state)
        {
            windowState = state;
        }
    }
}