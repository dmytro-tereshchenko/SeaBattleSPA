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
        /// Foreign key Id <see cref="GameShip"/>
        /// </summary>
        /// <value><see cref="int"/></value>
        int GameShipId { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="GameField"/>
        /// </summary>
        /// <value><see cref="int"/></value>
        int GameFieldId { get; set; }

        /// <summary>
        /// Navigate property <see cref="GameShip"/>
        /// </summary>
        /// <value><see cref="GameShip"/></value>
        GameShip GameShip { get; set; }

        /// <summary>
        /// Navigate property <see cref="GameField"/>
        /// </summary>
        /// <value><see cref="GameField"/></value>
        GameField GameField { get; set; }

        /// <summary>
        /// Label for stern of <see cref="GameShip"/>
        /// </summary>
        /// <value><see cref="bool"/>true - if it's coordinate of ship's stern, otherwise - false</value>
        bool Stern { get; set; }
    }
}
