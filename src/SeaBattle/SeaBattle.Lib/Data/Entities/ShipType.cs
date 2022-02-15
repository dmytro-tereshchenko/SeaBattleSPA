using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Types of ship
    /// </summary>
    public class ShipType : IShipType
    {
        [Key]
        public uint Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<IShip> Ships { get; set; } = new List<IShip>();
    }
}
