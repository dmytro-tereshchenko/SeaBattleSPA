namespace SeaBattle.Lib.Data
{
    /// <summary>
    /// Common player (user)
    /// </summary>
    public class Player
    {
        public uint Id { get; set; }

        public string Name { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class
        /// </summary>
        /// <param name="id">Player's id</param>
        /// <param name="name">Player's name</param>
        public Player(uint id, string name) : this(name) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class
        /// </summary>
        /// <param name="name">Player's name</param>
        public Player(string name)
        {
            Name = name;
        }
    }
}
