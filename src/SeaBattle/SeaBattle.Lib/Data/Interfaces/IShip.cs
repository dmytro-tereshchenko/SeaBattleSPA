using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic ship
    /// </summary>
    public interface IShip: ICommonShip, IEntity
    {
        /// <summary>
        /// Ship's cost
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint Cost { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="GameShip"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="GameShip"/>.</value>
        public ICollection<GameShip> GameShips { get; set; }
    }
}
