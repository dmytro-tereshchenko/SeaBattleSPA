using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Weapon equipment for ship
    /// </summary>
    public interface IWeapon : IEntity
    {
        /// <summary>
        /// Amount of hp that target ship can be damaged
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort Damage { get; set; }

        /// <summary>
        /// Distance to target ship which can be damaged.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort AttackRange { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="IGameShip"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"/>.</value>
        public ICollection<IGameShip> GameShips { get; set; }
    }
}
