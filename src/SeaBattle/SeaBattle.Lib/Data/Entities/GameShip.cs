using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        public ushort AttackRange
        {
            get => Weapons?.FirstOrDefault()?.AttackRange ?? 0;
        }

        [NotMapped]
        public ushort RepairRange
        {
            get => Repairs?.FirstOrDefault()?.RepairRange ?? 0;
        }

        [NotMapped]
        public ushort Damage
        {
            get => Convert.ToUInt16(Weapons?.Sum(w => w.Damage) ?? 0);
        }

        [NotMapped]
        public ushort RepairPower
        {
            get => Convert.ToUInt16(Repairs?.Sum(r => r.RepairPower) ?? 0);
        }

        [NotMapped]
        public ShipType ShipType
        {
            get => Ship.ShipType;
            set => Ship.ShipType = value;
        }

        [NotMapped]
        public byte Size
        {
            get => Ship.Size;
            set => Ship.Size = value;
        }

        [NotMapped]
        public ushort MaxHp
        {
            get => Ship.MaxHp;
            set => Ship.MaxHp = value;
        }

        [NotMapped]
        public byte Speed
        {
            get => Ship.Speed;
            set => Ship.Speed = value;
        }

        public int ShipId { get; set; }

        public int GamePlayerId { get; set; }

        public int? StartFieldId { get; set; }

        public Ship Ship { get; set; }

        public GamePlayer GamePlayer { get; set; }

        public StartField StartField { get; set; }

        public ICollection<Weapon> Weapons { get; set; }

        public ICollection<Repair> Repairs { get; set; }

        public ICollection<GameFieldCell> GameFieldCells { get; set; }

        public ICollection<StartFieldCell> StartFieldCells { get; set; }

        public ICollection<EquippedWeapon> EquippedWeapons { get; set; }

        public ICollection<EquippedRepair> EquippedRepairs { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameShip()
        {
            Weapons = new List<Weapon>();
            Repairs = new List<Repair>();
            GameFieldCells = new List<GameFieldCell>();
            StartFieldCells = new List<StartFieldCell>();
            EquippedWeapons = new List<EquippedWeapon>();
            EquippedRepairs = new List<EquippedRepair>();
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
