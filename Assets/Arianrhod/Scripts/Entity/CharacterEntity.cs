namespace Arianrhod.Entity
{
    public class CharacterEntity
    {
        public string Name { get; }
        public int Hp { get; private set; }
        public int Defence { get; }
        public int Range { get; }
        public int Dexterity { get; }
    }
}