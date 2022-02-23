using System.Collections.Generic;

namespace SeaBattle.GameResources.Dto
{
    public class GameFieldDto
    {
        public int Id { get; set; }

        public ushort SizeX { get; set; }

        public ushort SizeY { get; set; }

        public int GameId { get; set; }

        public ICollection<GameFieldCellDto> GameFieldCells { get; set; }
    }
}
