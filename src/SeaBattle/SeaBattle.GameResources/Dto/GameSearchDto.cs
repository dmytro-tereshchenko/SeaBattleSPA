namespace SeaBattle.GameResources.Dto
{
    public class GameSearchDto
    {
        public int Id { get; set; }

        public byte MaxNumberOfPlayers { get; set; }

        public byte GameState { get; set; }

        public string GameFieldSize { get; set; }

        public string Players { get; set; }
    }
}
