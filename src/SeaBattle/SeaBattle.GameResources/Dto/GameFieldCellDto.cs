namespace SeaBattle.GameResources.Dto
{
    public class GameFieldCellDto
    {
        public int Id { get; set; }

        public ushort X { get; set; }

        public ushort Y { get; set; }

        public bool Stern { get; set; }

        public int GameShipId { get; set; }

        public int PlayerId { get; set; }
    }
}
