using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    public class GameFieldCell : IGameFieldCell
    {
        public uint Id { get; set; }

        public ushort X { get; set; }

        public ushort Y { get; set; }

        public bool Stern { get; set; }

        [JsonIgnore]
        public uint GameShipId { get; set; }

        [JsonIgnore]
        public uint GameFieldId { get; set; }

        public GameShip GameShip { get; set; }

        [JsonIgnore]
        public GameField GameField { get; set; }
    }
}
