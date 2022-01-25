namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player (user) in the game
    /// </summary>
    public interface IPlayer : IEntity
    {
        /// <summary>
        /// Players name
        /// </summary>
        /// <value><see cref="string"/></value>
        public string Name { get; }

        /// <summary>
        /// Player's state when ready the game
        /// </summary>
        /// <value><see cref="string"/></value>
        public bool Ready { get; set; }
    }
}
