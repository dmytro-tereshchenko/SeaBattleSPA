using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.Lib.Entities
{
    public class GameFieldCell : IGameFieldCell
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ushort X { get; set; }

        [Required]
        public ushort Y { get; set; }

        [Required]
        public bool Stern { get; set; }

        public int GameShipId { get; set; }

        [Required]
        public int GameFieldId { get; set; }

        public GameShip GameShip { get; set; }

        public GameField GameField { get; set; }
    }
}
