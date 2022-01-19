using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public class Ship : IShip, IEntity
    {
        
        private uint _id;

        private ShipType _type;

        private byte _size;

        private ushort _maxHp;

        private byte _speed;

        private byte _equipmentSlots;

        protected ICollection<IWeapon> _weapons;

        protected ICollection<IRepair> _repairs;
        
        public uint Id { get => _id; }
        
        public ushort AttackRange { get => _weapons?.Max(w => w.AttackRange) ?? 0; }
        
        public ushort RepairRange { get => _repairs?.Max(r => r.RepairRange) ?? 0; }
        
        public ushort Damage { get => Convert.ToUInt16(_weapons?.Sum(w => w.Damage) ?? 0); }

        //amount of hp that ship can repair
        public ushort RepairPower { get => Convert.ToUInt16(_repairs?.Sum(r => r.RepairPower) ?? 0); }
        
        public ShipType Type { get => _type; }

        //length of the ship (cells), width = 1 cell
        public byte Size { get => _size; }
        
        public ushort MaxHp { get => _maxHp; }

        //Max speed (amount of cells, that the ship can move in 1 turn)
        public byte Speed { get => _speed; }

        //amount of slots to equip equipment
        public byte EquipmentSlots { get => _equipmentSlots; }
       
        public Ship(uint id, ShipType type, byte size, ushort maxHp, byte speed, byte eSlots)
        {
            this._id = id;
            this._type = type;
            this._size = size;
            this._maxHp = maxHp;
            this._speed = speed;
            _equipmentSlots = eSlots;
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
