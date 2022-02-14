using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Types of common ship.
    /// </summary>
    public interface IShipType : IEntity
    {
        /// <summary>
        /// Ship's name.
        /// </summary>
        /// <value><see cref="string"/></value>
        string Name { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="ICommonShip"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="ICommonShip"/>.</value>
        ICollection<ICommonShip> CommonShips { get; set; }
    }
}
