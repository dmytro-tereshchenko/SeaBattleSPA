using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    public interface ICommonShip: IEntity
    {
        ushort AttackRange { get; }

        ushort RepairRange { get; } 

        ushort Damage { get; }

        /// <summary>
        /// Amount of hp that ship can repair
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort RepairPower { get; }

        ShipType Type { get; }

        /// <summary>
        /// Length of the ship (cells) and amount of possible equipment slots, width = 1 cell
        /// </summary>
        /// <value><see cref="byte"/></value>
        byte Size { get; }

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
