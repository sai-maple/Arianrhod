namespace Arianrhod.Entity
{
    public class SkillEntity
    {
        public string Name { get; }
        public DiceType DiceType { get; }
        public int[,] Range { get; }
        public AnimationState AnimationState { get; }
    }
}