namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// The cell of field where ships are allocated
    /// </summary>
    public interface IGameFieldCell : IEntity
    {
        /// <summary>
        /// Coordinate X of game field
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort X { get; set; }

        /// <summary>
        /// Coordinate Y of game field
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort Y { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="IGameShip"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GameShipId { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="IGameField"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GameFieldId { get; set; }

        /// <summary>
        /// Navigate property <see cref="IGameShip/>
        /// </summary>
        /// <value><see cref="IGameShip"/></value>
        IGameShip GameShip { get; set; }

        /// <summary>
        /// Navigate property <see cref="IGameField/>
        /// </summary>
        /// <value><see cref="IGameField"/></value>
        IGameField GameField { get; set; }

        /// <summary>
        /// Label for stern of <see cref="IGameShip"/>
        /// </summary>
        /// <value><see cref="bool"/>true - if it's coordinate of ship's stern, otherwise - false</value>
        bool Stern { get; set; }
    }
}
