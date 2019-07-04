namespace Arianrhod.Entity
{
    public class SkillEntity
    {
        public SkillEntity(int name, int[,] range)
        {
            Name = name;
            Range = range;
        }

        public int Name { get; }
        public DiceType DiceType { get; }
        public int[,] Range { get; }
        public AnimationState AnimationState { get; }
    }
}