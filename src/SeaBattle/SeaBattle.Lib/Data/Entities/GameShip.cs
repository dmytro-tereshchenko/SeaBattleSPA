using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// An expanded ship that is used in the game
    /// </summary>
    public class GameShip : IGameShip
    {
        public uint Id { get; set; }

        public ushort Hp { get; set; }

        public int Points { get; set; }

        [NotMapped]
        [JsonIgnore]
        public ushort AttackRange
        {
            get => Weapons?.FirstOrDefault()?.AttackRange ?? 0;
        }

        [NotMapped]
        [JsonIgnore]
        public ushort RepairRange
        {
            get => Repairs?.FirstOrDefault()?.RepairRange ?? 0;
        }

        [NotMapped]
        [JsonIgnore]
        public ushort Damage
        {
            get => Convert.ToUInt16(Weapons?.Sum(w => w.Damage) ?? 0);
        }

        [NotMapped]
        [JsonIgnore]
        public ushort RepairPower
        {
            get => Convert.ToUInt16(Repairs?.Sum(r => r.RepairPower) ?? 0);
        }

        [NotMapped]
        [JsonIgnore]
        public IShipType ShipType
        {
            get => CommonShip.ShipType;
            set => CommonShip.ShipType = value;
        }

        [NotMapped]
        [JsonIgnore]
        public byte Size
        {
            get => CommonShip.Size;
            set => CommonShip.Size = value;
        }

        [NotMapped]
        [JsonIgnore]
        public ushort MaxHp
        {
            get => CommonShip.MaxHp;
            set => CommonShip.MaxHp = value;
        }

        [NotMapped]
        [JsonIgnore]
        public byte Speed
        {
            get => CommonShip.Speed;
            set => CommonShip.Speed = value;
        }

        [JsonIgnore]
        public uint CommonShipId { get; set; }

        [JsonIgnore]
        public uint GamePlayerId { get; set; }

        [JsonIgnore]
        public uint? StartFieldId { get; set; }

        [ForeignKey("CommonShipId")]
        public ICommonShip CommonShip { get; set; }

        [ForeignKey("GamePlayerId")]
        public IGamePlayer GamePlayer { get; set; }

        [JsonIgnore]
        [ForeignKey("StartFieldId")]
        public IStartField StartField { get; set; }

        public ICollection<IWeapon> Weapons { get; set; }

        public ICollection<IRepair> Repairs { get; set; }

        [JsonIgnore]
        public ICollection<IGameFieldCell> GameFieldCells { get; set; }

        [JsonIgnore]
        public ICollection<IStartFieldCell> StartFieldCells { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameShip()
        {
            Weapons = new List<IWeapon>();
            Repairs = new List<IRepair>();
            GameFieldCells = new List<IGameFieldCell>();
            StartFieldCells = new List<IStartFieldCell>();
        }

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
            CommonShip = ship;
            GamePlayer = gamePlayer;
            Points = points;
            Hp = hp;
            Weapons = new List<IWeapon>();
            Repairs = new List<IRepair>();
            GameFieldCells = new List<IGameFieldCell>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameShip"/> class
        /// </summary>
        /// <param name="ship">Basic ship</param>
        /// <param name="gamePlayer">The player who owns the ship</param>
        /// <param name="points">Ship's cost</param>
        public GameShip(ICommonShip ship, IGamePlayer gamePlayer, int points)
            : this(ship, gamePlayer, points, ship.MaxHp) { }

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

            return gameShip?.ShipType == this.ShipType && gameShip.Id == this.Id && gameShip.Speed == this.Speed &&
                   gameShip.Size == this.Size;
        }

        public override int GetHashCode()
        {
            return (Id + ShipType.ToString() + Speed + Size + MaxHp).GetHashCode() + base.GetHashCode();
        }
    }
}
