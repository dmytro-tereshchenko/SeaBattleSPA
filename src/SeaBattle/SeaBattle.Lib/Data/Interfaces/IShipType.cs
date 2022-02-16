using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Types of common ship.
    /// </summary>
    public interface IShipType : IShortEntity
    {
        /// <summary>
        /// Ship's name.
        /// </summary>
        /// <value><see cref="string"/></value>
        string Name { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="Ship"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="Ship"/>.</value>
        ICollection<Ship> Ships { get; set; }
    }
}
