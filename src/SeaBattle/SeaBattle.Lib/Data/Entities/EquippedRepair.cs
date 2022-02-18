using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Equipped to <see cref="GameShip"/> repairs
    /// </summary>
    public class EquippedRepair : IEquippedRepair
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RepairId { get; set; }

        public Repair Repair { get; set; }

        public int GameShipId { get; set; }

        public GameShip GameShip { get; set; }
    }
}
