using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.GameResources.Dto
{
    public class StartFieldDto
    {
        public int Id { get; set; }

        public int Points { get; set; }

        public int GameId { get; set; }

        public ICollection<StartFieldCellDto> StartFieldCells { get; set; }

        public ICollection<int> GameShipsId { get; set; }
    }
}
