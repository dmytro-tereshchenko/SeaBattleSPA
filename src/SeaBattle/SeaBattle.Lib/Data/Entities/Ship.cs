using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic ship
    /// </summary>
    public class Ship : IShip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        public short ShipTypeId { get; set; }

        [Required]
        public byte Size { get; set; }

        [Required]
        public ushort MaxHp { get; set; }

        [Required]
        public byte Speed { get; set; }

        [ForeignKey(nameof(ShipTypeId))]
        public ShipType ShipType { get; set; }

        [Required]
        public uint Cost { get; set; }

        [JsonIgnore]
        public ICollection<GameShip> GameShips { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Ship()
        {
            GameShips = new List<GameShip>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ship"/> class
        /// </summary>
        /// <param name="id">Ship's id</param>
        /// <param name="type">Type of ship</param>
        /// <param name="size">Length of the ship (cells) and amount of possible equipment slots</param>
        /// <param name="maxHp">Max hp of the ship that he can be damaged</param>
        /// <param name="speed">Max speed (amount of cells, that the ship can move in 1 turn)</param>
        public Ship(int id, ShipType type, byte size, ushort maxHp, byte speed)
        : this(type, size, maxHp, speed) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ship"/> class
        /// </summary>
        /// <param name="type">Type of ship</param>
        /// <param name="size">Length of the ship (cells) and amount of possible equipment slots</param>
        /// <param name="maxHp">Max hp of the ship that he can be damaged</param>
        /// <param name="speed">Max speed (amount of cells, that the ship can move in 1 turn)</param>
        public Ship(ShipType type, byte size, ushort maxHp, byte speed) : this()
        {
            ShipType = type;
            ShipTypeId = type.Id;
            Size = size;
            MaxHp = maxHp;
            Speed = speed;
        }
    }
}
