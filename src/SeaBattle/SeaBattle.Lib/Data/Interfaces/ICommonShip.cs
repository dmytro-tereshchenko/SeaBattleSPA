using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic ship
    /// </summary>
    public interface ICommonShip: IEntity
    {
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
        /// Type of ship
        /// </summary>
        /// <value><see cref="ShipType"/></value>
        ShipType Type { get; }

        /// <summary>
        /// Length of the ship (cells) and amount of possible equipment slots, width = 1 cell
        /// </summary>
        /// <value><see cref="byte"/></value>
        byte Size { get; }

        /// <summary>
        /// Max hp of the ship that he can be damaged
        /// </summary>
        /// <value><see cref="byte"/></value>
        ushort MaxHp { get; }

        /// <summary>
        /// Max speed (amount of cells, that the ship can move in 1 turn)
        /// </summary>
        /// <value><see cref="byte"/></value>
        byte Speed { get; }

        /// <summary>
        /// Collection of weapons on ship
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IWeapon"/></value>
        ICollection<IWeapon> Weapons { get; set; }

        /// <summary>
        /// Collection of weapons on ship
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IRepair"/></value>
        ICollection<IRepair> Repairs { get; set; }
    }
}
