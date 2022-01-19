using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaBattle.Lib.Entities
{
    public class Ship : IShip
    { 
        private uint _id;

        private ShipType _type;

        private byte _size;

        private ushort _maxHp;

        private byte _speed;

        protected ICollection<IWeapon> _weapons;

        protected ICollection<IRepair> _repairs;
        
        public uint Id { get => _id; }
        
        public ushort AttackRange { get => _weapons?.FirstOrDefault()?.AttackRange ?? 0; }
        
        public ushort RepairRange { get => _repairs?.FirstOrDefault()?.RepairRange ?? 0; }
        
        public ushort Damage { get => Convert.ToUInt16(_weapons?.Sum(w => w.Damage) ?? 0); }

        //amount of hp that ship can repair
        public ushort RepairPower { get => Convert.ToUInt16(_repairs?.Sum(r => r.RepairPower) ?? 0); }
        
        public ShipType Type { get => _type; }

        //length of the ship (cells) and amount of possible equipment slots, width = 1 cell
        public byte Size { get => _size; }
        
        public ushort MaxHp { get => _maxHp; }

        //Max speed (amount of cells, that the ship can move in 1 turn)
        public byte Speed { get => _speed; }
       
        public Ship(uint id, ShipType type, byte size, ushort maxHp, byte speed)
        {
            _id = id;
            _type = type;
            _size = size;
            _maxHp = maxHp;
            _speed = speed;
        }
        
        public void AddWeapon(IWeapon weapon)
        {
            if (_weapons == null)
                _weapons = new List<IWeapon>();
            _weapons.Add(weapon);
        }
        
        public void AddRepair(IRepair repair)
        {
            if (_repairs == null)
                _repairs = new List<IRepair>();
            _repairs.Add(repair);
        }
    }
}