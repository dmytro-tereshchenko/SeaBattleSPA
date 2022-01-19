using System;

namespace SeaBattle.Lib.Entities
{
    public class GameShip : IGameShip
    {
        private uint _id;

        private IShip _ship;

        private ushort _hp;

        private uint _teamId;

        private int _points;

        public uint Id { get => _id; }

        public IShip Ship { get => _ship; }

        //current hp
        public ushort Hp { get => _hp; set => _hp = Convert.ToUInt16(value < 0 ? 0 : value > MaxHp ? MaxHp : value); }
        
        public uint TeamId { get => _teamId; }
        
        public int Points { get => _points; }
        
        public ushort AttackRange { get => _ship.AttackRange; }
        
        public ushort RepairRange { get => _ship.RepairRange; }
        
        public ushort Damage { get => _ship.Damage; }

        //amount of hp that ship can repair
        public ushort RepairPower { get => _ship.RepairPower; }
        
        public ShipType Type { get => _ship.Type; }

        //length of the ship (cells) and amount of possible equipment slots, width = 1 cell
        public byte Size { get => _ship.Size; }
        
        public ushort MaxHp { get => _ship.MaxHp; }

        //Max speed (amount of cells, that the ship can move in 1 turn)
        public byte Speed { get => _ship.Speed; }
        
        public GameShip(uint id, IShip ship, uint teamId, int points, ushort hp)
        {
            _id = id;
            _ship = ship;
            _teamId = teamId;
            _points = points;
            _hp = hp;
        }
        
        public GameShip(uint id, IShip ship, uint teamId, int points) 
            : this(id, ship, teamId, points, ship.MaxHp) { }
    }
}
