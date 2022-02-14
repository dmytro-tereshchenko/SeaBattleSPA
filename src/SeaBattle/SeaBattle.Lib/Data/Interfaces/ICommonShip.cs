using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic ship
    /// </summary>
    public interface ICommonShip: IEntity
    {
        /// <summary>
        /// Foreign key Id <see cref="IShipType"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint ShipTypeId { get; set; }

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
        /// Navigate property <see cref="IShipType"/>
        /// </summary>
        /// <value><see cref="IShipType"/></value>
        IShipType ShipType { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="IGameShip"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"/>.</value>
        public ICollection<IGameShip> GameShips { get; set; }
    }
}
