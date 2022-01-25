using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// An expanded ship that is used in the game
    /// </summary>
    public class GameShip : IGameShip
    {
        public uint Id { get; set; }

        public ICommonShip Ship { get; private set; }

        public ushort Hp { get; set; }

        public IPlayer Player { get; private set; }

        public int Points { get; private set; }

        public ushort AttackRange { get => Ship.AttackRange; }

        public ushort RepairRange { get => Ship.RepairRange; }

        public ushort Damage { get => Ship.Damage; }

        public ushort RepairPower { get => Ship.RepairPower; }
        
        public ShipType Type { get => Ship.Type; }

        public byte Size { get => Ship.Size; }

        public ushort MaxHp { get => Ship.MaxHp; }

        public byte Speed { get => Ship.Speed; }

        public ICollection<IWeapon> Weapons { get=>Ship.Weapons; set=>Ship.Weapons=value; }

        public ICollection<IRepair> Repairs { get => Ship.Repairs; set => Ship.Repairs = value; }

        public GameShip(uint id, ICommonShip ship, IPlayer player, int points, ushort hp)
            : this(ship, player, points, hp) => Id = id;

        public GameShip(uint id, ICommonShip ship, IPlayer player, int points) 
            : this(id, ship, player, points, ship.MaxHp) { }

        public GameShip(ICommonShip ship, IPlayer player, int points, ushort hp)
        {
            Ship = ship;
            Player = player;
            Points = points;
            Hp = hp;
        }

        public GameShip(ICommonShip ship, IPlayer player, int points)
            : this(ship, player, points, ship.MaxHp) { }
    }
}
