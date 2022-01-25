namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player (user) in the game
    /// </summary>
    public class Player : IPlayer
    {
        /// <summary>
        /// Id Entity
        /// </summary>
        /// <value><see cref="uint"/></value>
        public uint Id { get; set; }

        /// <summary>
        /// Players name
        /// </summary>
        /// <value><see cref="string"/></value>
        public string Name { get; private set; }

        /// <summary>
        /// Player's state when ready the game
        /// </summary>
        /// <value><see cref="string"/></value>
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

#nullable enable
        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not Player)
            {
                return false;
            }

            Player player = (obj as Player)!;

            return player?.Name == this.Name && player.Id == this.Id;
        }
#nullable disable

        public override int GetHashCode()
        {
            return (Name + Id).GetHashCode() + base.GetHashCode();
        }
    }
}
