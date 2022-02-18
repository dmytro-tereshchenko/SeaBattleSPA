namespace SeaBattle.Lib.Infrastructure
{
    public class GameConfig : IGameConfig
    {
        public ushort MinFieldSizeX { get; private set; }

        public ushort MaxFieldSizeX { get; private set; }

        public ushort MinFieldSizeY { get; private set; }

        public ushort MaxFieldSizeY { get; private set; }

        public byte MaxNumberOfPlayers { get; private set; }

        public GameConfig(ushort minFieldSizeX, ushort maxFieldSizeX, ushort minFieldSizeY, ushort maxFieldSizeY, byte maxNumberOfPlayers)
        {
            MinFieldSizeX = minFieldSizeX;
            MaxFieldSizeX = maxFieldSizeX;
            MinFieldSizeY = minFieldSizeY;
            MaxFieldSizeY = maxFieldSizeY;
            MaxNumberOfPlayers = maxNumberOfPlayers;
        }
    }
}
