using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// An expanded ship that is used in the game
    /// </summary>
    public interface IGameShip : ICommonShip
    {
        /// <summary>
        /// Basic ship
        /// </summary>
        /// <value><see cref="ICommonShip"/></value>
        ICommonShip Ship { get; }

        /// <summary>
        /// Current hp of ship
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort Hp { get; set; }

        /// <summary>
        /// The player who owns this ship
        /// </summary>
        /// <value><see cref="IGamePlayer"/></value>
        IGamePlayer GamePlayer { get; }

        /// <summary>
        /// Number of ship cost points
        /// </summary>
        /// <value><see cref="int"/></value>
        int Points { get; }

        /// <summary>
        /// Distance to target ship which can be damaged.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort AttackRange { get; }

        /// <summary>
        /// Distance to target ship which can be repaired.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort RepairRange { get; }

        /// <summary>
        /// Amount of hp that target ship can be damaged
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort Damage { get; }

        /// <summary>
        /// Amount of hp that ship can be repaired
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort RepairPower { get; }

        /// <summary>
        /// Collection of weapons on ship
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IWeapon"/></value>
        ICollection<IWeapon> Weapons { get; }

        /// <summary>
        /// Collection of weapons on ship
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IRepair"/></value>
        ICollection<IRepair> Repairs { get; }
    }
}
