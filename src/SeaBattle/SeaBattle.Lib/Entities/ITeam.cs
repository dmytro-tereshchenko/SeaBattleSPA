namespace SeaBattle.Lib.Entities
{
    public interface ITeam : IEntity
    {
        public string Name { get; }

        public uint GameId { get; }

        public bool Ready { get; set; }
    }
}