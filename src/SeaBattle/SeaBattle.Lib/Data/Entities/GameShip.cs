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

        /// <summary>
        /// Initializes a new instance of the <see cref="GameShip"/> class
        /// </summary>
        /// <param name="id">Id of ship</param>
        /// <param name="ship">Basic ship</param>
        /// <param name="gamePlayer">The player who owns the ship</param>
        /// <param name="points">Ship's cost</param>
        /// <param name="hp">Current hp of ship</param>
        public GameShip(uint id, ICommonShip ship, IGamePlayer gamePlayer, int points, ushort hp)
            : this(ship, gamePlayer, points, hp) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameShip"/> class
        /// </summary>
        /// <param name="id">Id of ship</param>
        /// <param name="ship">Basic ship</param>
        /// <param name="gamePlayer">The player who owns the ship</param>
        /// <param name="points">Ship's cost</param>
        public GameShip(uint id, ICommonShip ship, IGamePlayer gamePlayer, int points) 
            : this(id, ship, gamePlayer, points, ship.MaxHp) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameShip"/> class
        /// </summary>
        /// <param name="ship">Basic ship</param>
        /// <param name="gamePlayer">The player who owns the ship</param>
        /// <param name="points">Ship's cost</param>
        /// <param name="hp">Current hp of ship</param>
        public GameShip(ICommonShip ship, IGamePlayer gamePlayer, int points, ushort hp)
        {
            Ship = ship;
            GamePlayer = gamePlayer;
            Points = points;
            Hp = hp;
            Weapons = new List<IWeapon>();
            Repairs = new List<IRepair>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameShip"/> class
        /// </summary>
        /// <param name="ship">Basic ship</param>
        /// <param name="gamePlayer">The player who owns the ship</param>
        /// <param name="points">Ship's cost</param>
        public GameShip(ICommonShip ship, IGamePlayer gamePlayer, int points)
            : this(ship, gamePlayer, points, ship.MaxHp) { }

        public GameShip() { }

        public static bool operator ==(GameShip obj1, GameShip obj2) =>
            obj1?.Equals(obj2) ?? false;

        public static bool operator !=(GameShip obj1, GameShip obj2) =>
            !(obj1 == obj2);

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not GameShip)
            {
                return false;
            }

            GameShip gameShip = (obj as GameShip)!;

            return gameShip?.Type == this.Type && gameShip.Id == this.Id && gameShip.Speed == this.Speed &&
                   gameShip.Size == this.Size;
        }

        public override int GetHashCode()
        {
            return (Id + Type.ToString() + Speed + Size + MaxHp).GetHashCode() + base.GetHashCode();
        }
    }
}
