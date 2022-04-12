namespace SeaBattle.GameResources.Dto
{
    public class GameSizeLimitDto
    {
        public byte MaxPlayerSize { get; set; }

        public ushort FieldMaxSizeX { get; set; }

        public ushort FieldMaxSizeY { get; set; }

        public ushort FieldMinSizeX { get; set; }

        public ushort FieldMinSizeY { get; set; }
    }
}
