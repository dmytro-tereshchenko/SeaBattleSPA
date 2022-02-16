using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic repair equipment for ship
    /// </summary>
    public class Repair : IRepair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        public ushort RepairPower { get; set; }

        [Required]
        public ushort RepairRange { get; set; }

        [JsonIgnore]
        public ICollection<GameShip> GameShips { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Repair()
        {
            GameShips = new List<GameShip>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repair"/> class
        /// </summary>
        /// <param name="id">Id of repair</param>
        /// <param name="power">Amount of hp that ship can repair</param>
        /// <param name="range">Distance to target ship which can be repaired</param>
        public Repair(int id, ushort power, ushort range)
            : this(power, range) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repair"/> class
        /// </summary>
        /// <param name="power">Amount of hp that ship can repair</param>
        /// <param name="range">Distance to target ship which can be repaired</param>
        public Repair(ushort power, ushort range) : this()
        {
            RepairPower = power;
            RepairRange = range;
        }
    }
}
