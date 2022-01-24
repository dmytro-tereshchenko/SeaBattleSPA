namespace SeaBattle.Lib.Entities
{
    public interface IPlayer : IEntity
    {
        public string Name { get; }

        public uint GameId { get; }

        public bool Ready { get; set; }
    }
}
