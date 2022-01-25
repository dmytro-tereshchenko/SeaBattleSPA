namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// An expanded ship that is used in the game
    /// </summary>
    public interface IGameShip : ICommonShip
    {
        /// <summary>
        /// Basic ship
        /// </summary>
        /// <value><see cref="ICommonShip"/></value>
        ICommonShip Ship { get; }

        /// <summary>
        /// Current hp of ship
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort Hp { get; set; }

        /// <summary>
        /// The player who owns this ship
        /// </summary>
        /// <value><see cref="IPlayer"/></value>
        IPlayer Player { get; }

        /// <summary>
        /// Number of ship cost points
        /// </summary>
        /// <value><see cref="int"/></value>
        int Points { get; }
    }
}
