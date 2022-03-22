namespace SeaBattle.GameResources.Dto
{
    public class MoveShipDto
    {
        public int GameShipId { get; set; }

        public ushort TPosX { get; set; }

        public ushort TPosY { get; set; }

        public byte Direction { get; set; }

        public int GameFieldId { get; set; }
    }
}
