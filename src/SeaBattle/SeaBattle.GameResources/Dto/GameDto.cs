using SeaBattle.Lib.Entities;
using System.Collections.Generic;

namespace SeaBattle.GameResources.Dto
{
    public class GameDto
    {
        public int Id { get; set; }

        public string CurrentPlayerMove { get; set; }

        public byte MaxNumberOfPlayers { get; set; }

        public byte GameState { get; set; }

        public ICollection<PlayerDto> Players { get; set; }
    }
}
