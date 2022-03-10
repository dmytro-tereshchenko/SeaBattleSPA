namespace SeaBattle.GameResources.Dto
{
    public class ShipDto
    {
        public int Id { get; set; }

        public byte Size { get; set; }

        public ushort MaxHp { get; set; }

        public byte Speed { get; set; }

        public byte ShipType { get; set; }

        public uint Cost { get; set; }
    }
}
