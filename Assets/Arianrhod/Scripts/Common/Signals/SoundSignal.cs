namespace Arianrhod
{
    public class SoundSignal
    {
        public SoundEffect soundEffect { get; }

        public SoundSignal(SoundEffect sound)
        {
            soundEffect = sound;
        }
    }
}