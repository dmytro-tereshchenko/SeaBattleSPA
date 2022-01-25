using System;
using System.Collections.Generic;
using System.Linq;

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

        public IGamePlayer GamePlayer { get; private set; }

        public int Points { get; private set; }

        public ushort AttackRange { get => Weapons?.FirstOrDefault()?.AttackRange ?? 0; }

        public ushort RepairRange { get => Repairs?.FirstOrDefault()?.RepairRange ?? 0; }

        public ushort Damage { get => Convert.ToUInt16(Weapons?.Sum(w => w.Damage) ?? 0); }

        public ushort RepairPower { get => Convert.ToUInt16(Repairs?.Sum(r => r.RepairPower) ?? 0); }

        public ShipType Type { get => Ship.Type; }

        public byte Size { get => Ship.Size; }

        public ushort MaxHp { get => Ship.MaxHp; }

        public byte Speed { get => Ship.Speed; }

        public ICollection<IWeapon> Weapons { get; private set; }

        public ICollection<IRepair> Repairs { get; private set; }

        public GameShip(uint id, ICommonShip ship, IGamePlayer gamePlayer, int points, ushort hp)
            : this(ship, gamePlayer, points, hp) => Id = id;

        public GameShip(uint id, ICommonShip ship, IGamePlayer gamePlayer, int points) 
            : this(id, ship, gamePlayer, points, ship.MaxHp) { }

        public GameShip(ICommonShip ship, IGamePlayer gamePlayer, int points, ushort hp)
        {
            Ship = ship;
            GamePlayer = gamePlayer;
            Points = points;
            Hp = hp;
            Weapons = new List<IWeapon>();
            Repairs = new List<IRepair>();
        }

        public GameShip(ICommonShip ship, IGamePlayer gamePlayer, int points)
            : this(ship, gamePlayer, points, ship.MaxHp) { }
    }
}
