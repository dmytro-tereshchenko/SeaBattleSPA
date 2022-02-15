using System.Collections.Generic;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic ship
    /// </summary>
    public class Ship : IShip
    {
        public uint Id { get; set; }

        [JsonIgnore]
        public uint ShipTypeId { get; set; }

        public byte Size { get; set; }

        public ushort MaxHp { get; set; }

        public byte Speed { get; set; }

        public IShipType ShipType { get; set; }

        [JsonIgnore]
        public ICollection<IGameShip> GameShips { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Ship()
        {
            GameShips = new List<IGameShip>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ship"/> class
        /// </summary>
        /// <param name="id">Ship's id</param>
        /// <param name="type">Type of ship</param>
        /// <param name="size">Length of the ship (cells) and amount of possible equipment slots</param>
        /// <param name="maxHp">Max hp of the ship that he can be damaged</param>
        /// <param name="speed">Max speed (amount of cells, that the ship can move in 1 turn)</param>
        public Ship(uint id, IShipType type, byte size, ushort maxHp, byte speed)
        : this(type, size, maxHp, speed) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ship"/> class
        /// </summary>
        /// <param name="type">Type of ship</param>
        /// <param name="size">Length of the ship (cells) and amount of possible equipment slots</param>
        /// <param name="maxHp">Max hp of the ship that he can be damaged</param>
        /// <param name="speed">Max speed (amount of cells, that the ship can move in 1 turn)</param>
        public Ship(IShipType type, byte size, ushort maxHp, byte speed) : this()
        {
            ShipType = type;
            ShipTypeId = type.Id;
            Size = size;
            MaxHp = maxHp;
            Speed = speed;
        }
    }
}
