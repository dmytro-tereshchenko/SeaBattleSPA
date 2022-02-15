using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic ship
    /// </summary>
    public interface IShip: ICommonShip, IEntity
    {
        /// <summary>
        /// Foreign key Id <see cref="IShipType"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint ShipTypeId { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="IGameShip"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"/>.</value>
        public ICollection<IGameShip> GameShips { get; set; }
    }
}
