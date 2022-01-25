using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic ship
    /// </summary>
    public class Ship : ICommonShip
    {
        /// <summary>
        /// Id Entity
        /// </summary>
        /// <value><see cref="uint"/></value>
        public uint Id { get; set; }

        /// <summary>
        /// Distance to target ship which can be damaged.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort AttackRange { get => Weapons?.FirstOrDefault()?.AttackRange ?? 0; }

        /// <summary>
        /// Distance to target ship which can be repaired.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort RepairRange { get => Repairs?.FirstOrDefault()?.RepairRange ?? 0; }

        /// <summary>
        /// Amount of hp that target ship can be damaged
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort Damage { get => Convert.ToUInt16(Weapons?.Sum(w => w.Damage) ?? 0); }

        /// <summary>
        /// Amount of hp that ship can repair
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort RepairPower { get => Convert.ToUInt16(Repairs?.Sum(r => r.RepairPower) ?? 0); }
        
        /// <summary>
        /// Type of ship
        /// </summary>
        /// <value><see cref="ShipType"/></value>
        public ShipType Type { get; private set; }

        /// <summary>
        /// Length of the ship (cells) and amount of possible equipment slots, width = 1 cell
        /// </summary>
        /// <value><see cref="byte"/></value>
        public byte Size { get; private set; }

        /// <summary>
        /// Max hp of the ship that he can be damaged
        /// </summary>
        /// <value><see cref="byte"/></value>
        public ushort MaxHp { get; private set; }

        /// <summary>
        /// Max speed (amount of cells, that the ship can move in 1 turn)
        /// </summary>
        /// <value><see cref="byte"/></value>
        public byte Speed { get; private set; }

        /// <summary>
        /// Collection of weapons on ship
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IWeapon"/></value>
        public ICollection<IWeapon> Weapons { get; set; }

        /// <summary>
        /// Collection of weapons on ship
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IRepair"/></value>
        public ICollection<IRepair> Repairs { get; set; }

        public Ship(uint id, ShipType type, byte size, ushort maxHp, byte speed)
        : this(type, size, maxHp, speed) => Id = id;

        public Ship(ShipType type, byte size, ushort maxHp, byte speed)
        {
            Type = type;
            Size = size;
            MaxHp = maxHp;
            Speed = speed;
            Weapons = new List<IWeapon>();
            Repairs = new List<IRepair>();
        }
    }
}
