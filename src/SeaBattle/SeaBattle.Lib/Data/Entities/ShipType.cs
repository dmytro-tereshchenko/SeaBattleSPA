using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Types of ship
    /// </summary>
    public class ShipType : IShipType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Ship> Ships { get; set; } = new List<Ship>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ShipType(){}
    }
}
