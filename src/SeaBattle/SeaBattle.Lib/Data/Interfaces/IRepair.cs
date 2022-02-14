using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Repair equipment for ship
    /// </summary>
    public interface IRepair : IEntity
    {
        /// <summary>
        /// Amount of hp that ship can repair
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort RepairPower { get; set; }

        /// <summary>
        /// Distance to target ship which can be repaired.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort RepairRange { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="IGameShip"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"/>.</value>
        public ICollection<IGameShip> GameShips { get; set; }
    }
}
