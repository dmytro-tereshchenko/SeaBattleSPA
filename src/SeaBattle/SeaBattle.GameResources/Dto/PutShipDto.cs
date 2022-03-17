namespace SeaBattle.GameResources.Dto
{
    public class PutShipDto
    {
        public ushort tPosX { get; set; }

        public ushort tPosY { get; set; }

        public byte direction { get; set; }

        public int startFieldId { get; set; }

        public int shipId { get; set; }
    }
}
