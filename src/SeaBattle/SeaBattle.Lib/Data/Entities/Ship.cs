namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic ship
    /// </summary>
    public class Ship : ICommonShip
    {
        public uint Id { get; set; }

        public ShipType Type { get; private set; }

        public byte Size { get; private set; }

        public ushort MaxHp { get; private set; }

        public byte Speed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ship"/> class
        /// </summary>
        /// <param name="id">Ship's id</param>
        /// <param name="type">Type of ship</param>
        /// <param name="size">Length of the ship (cells) and amount of possible equipment slots</param>
        /// <param name="maxHp">Max hp of the ship that he can be damaged</param>
        /// <param name="speed">Max speed (amount of cells, that the ship can move in 1 turn)</param>
        public Ship(uint id, ShipType type, byte size, ushort maxHp, byte speed)
        : this(type, size, maxHp, speed) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ship"/> class
        /// </summary>
        /// <param name="type">Type of ship</param>
        /// <param name="size">Length of the ship (cells) and amount of possible equipment slots</param>
        /// <param name="maxHp">Max hp of the ship that he can be damaged</param>
        /// <param name="speed">Max speed (amount of cells, that the ship can move in 1 turn)</param>
        public Ship(ShipType type, byte size, ushort maxHp, byte speed)
        {
            Type = type;
            Size = size;
            MaxHp = maxHp;
            Speed = speed;
        }
    }
}
