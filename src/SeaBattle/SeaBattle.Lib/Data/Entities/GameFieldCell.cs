using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    public class GameFieldCell : IGameFieldCell
    {
        public uint Id { get; set; }

        public ushort X { get; set; }

        public ushort Y { get; set; }

        [JsonIgnore]
        public uint GameShipId { get; set; }

        [JsonIgnore]
        public uint GameFieldId { get; set; }

        [ForeignKey("GameShipId")]
        public IGameShip GameShip { get; set; }

        [JsonIgnore]
        [ForeignKey("GameFieldId")]
        public IGameField GameField { get; set; }

        public bool Stern { get; set; }
    }
}
