using System.Collections.Generic;

namespace Arianrhod.Entity
{
    public class CharacterEntity
    {
        public string Name { get; }
        public int Hp { get; }
        public int Defence { get; }
        public int Range { get; }
        public int Dexterity { get; }
        public List<SkillEntity> SkillEntities = new List<SkillEntity>();

        public int Id;
        public Owner Owner;
    }
}