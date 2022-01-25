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
        public uint Id { get; set; }

        public ushort AttackRange { get => Weapons?.FirstOrDefault()?.AttackRange ?? 0; }

        public ushort RepairRange { get => Repairs?.FirstOrDefault()?.RepairRange ?? 0; }

        public ushort Damage { get => Convert.ToUInt16(Weapons?.Sum(w => w.Damage) ?? 0); }

        public ushort RepairPower { get => Convert.ToUInt16(Repairs?.Sum(r => r.RepairPower) ?? 0); }
        
        public ShipType Type { get; private set; }

        public byte Size { get; private set; }

        public ushort MaxHp { get; private set; }

        public byte Speed { get; private set; }

        public ICollection<IWeapon> Weapons { get; set; }

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
