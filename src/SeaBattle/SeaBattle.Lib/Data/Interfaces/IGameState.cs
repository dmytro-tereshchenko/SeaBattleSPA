namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Possible states of the game.
    /// </summary>
    public interface IGameState : IEntity
    {
        /// <summary>
        /// Name of the game state.
        /// </summary>
        /// <value><see cref="string"/></value>
        string Name { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="IGame"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GameId { get; set; }

        /// <summary>
        /// Navigate property <see cref="IGame/>
        /// </summary>
        /// <value><see cref="IGame"/></value>
        IGame Game { get; set; }
    }
}
