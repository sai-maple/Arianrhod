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
        public int[,] Range { get; }
    }
}