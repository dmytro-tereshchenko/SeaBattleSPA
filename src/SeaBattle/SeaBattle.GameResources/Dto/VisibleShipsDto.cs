using Microsoft.AspNetCore.Mvc;

namespace SeaBattle.GameResources.Dto
{
    [BindProperties]
    public class VisibleShipsDto
    {
        public int GameShipId { get; set; }

        public int GameFieldId { get; set; }

        public byte Action { get; set; }
    }
}
