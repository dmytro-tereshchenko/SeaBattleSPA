namespace SeaBattle.Lib.Entities
{
    public class Player : IPlayer
    {
        public uint Id { get; set; }

        public string Name { get; private set; }

        public uint GameId { get; private set; }

        public bool Ready { get; set; }

        public Player(uint id, string name, uint gameId) : this(name, gameId) => Id = id;

        public Player(string name, uint gameId)
        {
            Name = name;
            GameId = gameId;
        }
    }
}
