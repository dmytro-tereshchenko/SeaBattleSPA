using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public class GameShip : IGameShip
    {
        private uint _id;

        private IShip _ship;

        private ushort _hp;

        private string _team;

        private int _points;

        public uint Id { get => _id; }

        public IShip Ship { get => _ship; }

        //current hp
        public ushort Hp { get => _hp; set => _hp = Convert.ToUInt16(value < 0 ? 0 : value > MaxHp ? MaxHp : value); }
        
        public string Team { get => _team; }
        
        public int Points { get => _points; }
        
        public ushort AttackRange { get => _ship.AttackRange; }
        
        public ushort RepairRange { get => _ship.RepairRange; }
        
        public ushort Damage { get => _ship.Damage; }

        //amount of hp that ship can repair
        public ushort RepairPower { get => _ship.RepairPower; }
        
        public ShipType Type { get => _ship.Type; }

        //length of the ship (cells), width = 1 cell
        public byte Size { get => _ship.Size; }
        
        public ushort MaxHp { get => _ship.MaxHp; }

        //Max speed (amount of cells, that the ship can move in 1 turn)
        public byte Speed { get => _ship.Speed; }

        //amount of slots to equip equipment
        public byte EquipmentSlots { get => _ship.EquipmentSlots; }
        
        public GameShip(uint id, IShip ship, string team, int points, ushort hp)
        {
            this._id = id;
            this._ship = ship;
            this._team = team;
            this._points = points;
            this._hp = hp;
        }
        
        public GameShip(uint id, IShip ship, string team, int points) 
            : this(id, ship, team, points, ship.MaxHp) { }
    }
}
