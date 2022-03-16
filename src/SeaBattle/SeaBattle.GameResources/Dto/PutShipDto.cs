namespace SeaBattle.GameResources.Dto
{
    public class PutShipDto
    {
        public ushort TPosX { get; set; }

        public ushort TPosY { get; set; }

        public byte Direction { get; set; }

        public int StartFieldId { get; set; }

        public int ShipId { get; set; }
    }
}
