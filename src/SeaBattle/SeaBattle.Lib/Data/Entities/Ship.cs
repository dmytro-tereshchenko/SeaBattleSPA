using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaBattle.Lib.Entities
{
    public class Ship : IShip
    { 
        protected ICollection<IWeapon> _weapons;

        protected ICollection<IRepair> _repairs;

        public uint Id { get; set; }

        public ushort AttackRange { get => _weapons?.FirstOrDefault()?.AttackRange ?? 0; }
        
        public ushort RepairRange { get => _repairs?.FirstOrDefault()?.RepairRange ?? 0; }
        
        public ushort Damage { get => Convert.ToUInt16(_weapons?.Sum(w => w.Damage) ?? 0); }

        /// <summary>
        /// Amount of hp that ship can repair
        /// </summary>
        public ushort RepairPower { get => Convert.ToUInt16(_repairs?.Sum(r => r.RepairPower) ?? 0); }
        
        public ShipType Type { get; private set; }

        /// <summary>
        /// Length of the ship (cells) and amount of possible equipment slots, width = 1 cell
        /// </summary>
        public byte Size { get; private set; }

        public ushort MaxHp { get; private set; }

        /// <summary>
        /// Max speed (amount of cells, that the ship can move in 1 turn)
        /// </summary>
        public byte Speed { get; private set; }

        public Ship(uint id, ShipType type, byte size, ushort maxHp, byte speed)
        : this(type, size, maxHp, speed) => Id = id;

        public Ship(ShipType type, byte size, ushort maxHp, byte speed)
        {
            Type = type;
            Size = size;
            MaxHp = maxHp;
            Speed = speed;
        }

        public void AddWeapon(IWeapon weapon)
        {
            if (_weapons == null)
            {
                _weapons = new List<IWeapon>();
            }

            _weapons.Add(weapon);
        }
        
        public void AddRepair(IRepair repair)
        {
            if (_repairs == null)
            {
                _repairs = new List<IRepair>();
            }

            _repairs.Add(repair);
        }

        public bool RemoveWeapon(IWeapon weapon) => _weapons.Remove(weapon);

        public bool RemoveRepair(IRepair repair) => _repairs.Remove(repair);
    }
}
