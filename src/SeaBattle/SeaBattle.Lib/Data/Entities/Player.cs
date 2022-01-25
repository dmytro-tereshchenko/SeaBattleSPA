namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player (user) in the game
    /// </summary>
    public class Player : IPlayer
    {
        public uint Id { get; set; }

        public string Name { get; private set; }

        public bool Ready { get; set; }

        public Player(uint id, string name) : this(name) => Id = id;

        public Player(string name)
        {
            Name = name;
        }

        public static bool operator ==(Player obj1, Player obj2) =>
            obj1?.Equals(obj2) ?? false;

        public static bool operator !=(Player obj1, Player obj2) =>
            !(obj1==obj2);

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not Player)
            {
                return false;
            }

            Player player = (obj as Player)!;

            return player?.Name == this.Name && player.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return (Name + Id).GetHashCode() + base.GetHashCode();
        }
    }
}
