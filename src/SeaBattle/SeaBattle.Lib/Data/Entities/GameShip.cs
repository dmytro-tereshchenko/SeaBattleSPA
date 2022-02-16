using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ushort Hp { get; set; }

        [Required]
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
        public ShipType ShipType
        {
            get => Ship.ShipType;
            set => Ship.ShipType = value;
        }

        [NotMapped]
        [JsonIgnore]
        public byte Size
        {
            get => Ship.Size;
            set => Ship.Size = value;
        }

        [NotMapped]
        [JsonIgnore]
        public ushort MaxHp
        {
            get => Ship.MaxHp;
            set => Ship.MaxHp = value;
        }

        [NotMapped]
        [JsonIgnore]
        public byte Speed
        {
            get => Ship.Speed;
            set => Ship.Speed = value;
        }

        [JsonIgnore]
        public int ShipId { get; set; }

        [JsonIgnore]
        public int GamePlayerId { get; set; }

        [JsonIgnore]
        public int? StartFieldId { get; set; }

        public Ship Ship { get; set; }

        public GamePlayer GamePlayer { get; set; }

        [JsonIgnore]
        public StartField StartField { get; set; }

        public ICollection<Weapon> Weapons { get; set; }

        public ICollection<Repair> Repairs { get; set; }

        [JsonIgnore]
        public ICollection<GameFieldCell> GameFieldCells { get; set; }

        [JsonIgnore]
        public ICollection<StartFieldCell> StartFieldCells { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameShip()
        {
            Weapons = new List<Weapon>();
            Repairs = new List<Repair>();
            GameFieldCells = new List<GameFieldCell>();
            StartFieldCells = new List<StartFieldCell>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameShip"/> class
        /// </summary>
        /// <param name="id">Id of ship</param>
        /// <param name="ship">Basic ship</param>
        /// <param name="gamePlayer">The player who owns the ship</param>
        /// <param name="points">Ship's cost</param>
        /// <param name="hp">Current hp of ship</param>
        public GameShip(int id, Ship ship, GamePlayer gamePlayer, int points, ushort hp)
            : this(ship, gamePlayer, points, hp) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameShip"/> class
        /// </summary>
        /// <param name="id">Id of ship</param>
        /// <param name="ship">Basic ship</param>
        /// <param name="gamePlayer">The player who owns the ship</param>
        /// <param name="points">Ship's cost</param>
        public GameShip(int id, Ship ship, GamePlayer gamePlayer, int points)
            : this(id, ship, gamePlayer, points, ship.MaxHp) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameShip"/> class
        /// </summary>
        /// <param name="ship">Basic ship</param>
        /// <param name="gamePlayer">The player who owns the ship</param>
        /// <param name="points">Ship's cost</param>
        /// <param name="hp">Current hp of ship</param>
        public GameShip(Ship ship, GamePlayer gamePlayer, int points, ushort hp) : this()
        {
            Ship = ship;
            GamePlayer = gamePlayer;
            Points = points;
            Hp = hp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameShip"/> class
        /// </summary>
        /// <param name="ship">Basic ship</param>
        /// <param name="gamePlayer">The player who owns the ship</param>
        /// <param name="points">Ship's cost</param>
        public GameShip(Ship ship, GamePlayer gamePlayer, int points)
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
