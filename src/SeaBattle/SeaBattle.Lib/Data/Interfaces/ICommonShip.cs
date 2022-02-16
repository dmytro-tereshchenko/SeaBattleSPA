namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Generic ship
    /// </summary>
    public interface ICommonShip
    {
        /// <summary>
        /// Length of the ship (cells) and amount of possible equipment slots, width = 1 cell
        /// </summary>
        /// <value><see cref="byte"/></value>
        byte Size { get; set; }

        /// <summary>
        /// Max hp of the ship that he can be damaged
        /// </summary>
        /// <value><see cref="byte"/></value>
        ushort MaxHp { get; set; }

        /// <summary>
        /// Max speed (amount of cells, that the ship can move in 1 turn)
        /// </summary>
        /// <value><see cref="byte"/></value>
        byte Speed { get; set; }

        /// <summary>
        /// Navigate property <see cref="ShipType"/>
        /// </summary>
        /// <value><see cref="ShipType"/></value>
        ShipType ShipType { get; set; }
    }
}
