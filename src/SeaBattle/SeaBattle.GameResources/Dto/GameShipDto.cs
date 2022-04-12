using System.Collections.Generic;

namespace SeaBattle.GameResources.Dto
{
    public class GameShipDto
    {
        public int Id { get; set; }

        public ushort Hp { get; set; }

        public byte ShipType { get; set; }

        public byte Size { get; set; }

        public ushort MaxHp { get; set; }

        public byte Speed { get; set; }

        public int GamePlayerId { get; set; }

        public ICollection<WeaponDto> Weapons { get; set; }

        public ICollection<RepairDto> Repairs { get; set; }
    }
}
