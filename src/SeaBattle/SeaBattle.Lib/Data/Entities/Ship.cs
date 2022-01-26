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

        public Ship(uint id, ShipType type, byte size, ushort maxHp, byte speed)
        : this(type, size, maxHp, speed) => Id = id;

        public Ship(ShipType type, byte size, ushort maxHp, byte speed)
        {
            Type = type;
            Size = size;
            MaxHp = maxHp;
            Speed = speed;
        }
    }
}
