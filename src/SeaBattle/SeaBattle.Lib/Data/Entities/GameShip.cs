using System;

namespace SeaBattle.Lib.Entities
{
    public class GameShip : IGameShip
    {
        public uint Id { get; set; }

        public IShip Ship { get; private set; }

        //current hp
        public ushort Hp { get; set; }

        public uint TeamId { get; private set; }

        public int Points { get; private set; }

        public ushort AttackRange { get => Ship.AttackRange; }
        
        public ushort RepairRange { get => Ship.RepairRange; }
        
        public ushort Damage { get => Ship.Damage; }

        //amount of hp that ship can repair
        public ushort RepairPower { get => Ship.RepairPower; }
        
        public ShipType Type { get => Ship.Type; }

        //length of the ship (cells) and amount of possible equipment slots, width = 1 cell
        public byte Size { get => Ship.Size; }
        
        public ushort MaxHp { get => Ship.MaxHp; }

        //Max speed (amount of cells, that the ship can move in 1 turn)
        public byte Speed { get => Ship.Speed; }

        public GameShip(uint id, IShip ship, uint teamId, int points, ushort hp)
            : this(ship, teamId, points, hp) => Id = id;

        public GameShip(uint id, IShip ship, uint teamId, int points) 
            : this(id, ship, teamId, points, ship.MaxHp) { }

        public GameShip(IShip ship, uint teamId, int points, ushort hp)
        {
            Ship = ship;
            TeamId = teamId;
            Points = points;
            Hp = hp;
        }

        public GameShip(IShip ship, uint teamId, int points)
            : this(ship, teamId, points, ship.MaxHp) { }
    }
}
