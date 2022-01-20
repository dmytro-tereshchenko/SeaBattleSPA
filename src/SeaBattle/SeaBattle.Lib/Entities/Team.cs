namespace SeaBattle.Lib.Entities
{
    public class Team : ITeam
    {
        private uint _id;

        private string _name;

        private uint _gameId;

        private bool _ready;

        public uint Id { get => _id; }

        public string Name { get => _name; }

        public uint GameId { get => _gameId; }

        public bool Ready { get => _ready; set => _ready = value; }

        public Team(uint id, string name, uint gameId)
        {
            _id = id;
            _name = name;
            _gameId = gameId;
        }
    }
}
