using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    public class GameFieldCell : IGameFieldCell
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ushort X { get; set; }

        public ushort Y { get; set; }

        public bool Stern { get; set; }

        [JsonIgnore]
        public int GameShipId { get; set; }

        [JsonIgnore]
        public int GameFieldId { get; set; }

        public GameShip GameShip { get; set; }

        [JsonIgnore]
        public GameField GameField { get; set; }
    }
}
