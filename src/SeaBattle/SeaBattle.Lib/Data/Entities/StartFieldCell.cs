using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// The field's cell for storing the location of ships when initializing game.
    /// </summary>
    public class StartFieldCell : IStartFieldCell
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ushort X { get; set; }

        [Required]
        public ushort Y { get; set; }

        public int? StartFieldId { get; set; }

        public StartField StartField { get; set; }
    }
}
