using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic repair equipment for ship
    /// </summary>
    public class BasicRepair : IRepair
    {
        [Key]
        public uint Id { get; set; }

        [Required]
        public ushort RepairPower { get; set; }

        [Required]
        public ushort RepairRange { get; set; }

        [JsonIgnore]
        public ICollection<IGameShip> GameShips { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasicRepair()
        {
            GameShips = new List<IGameShip>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicRepair"/> class
        /// </summary>
        /// <param name="id">Id of repair</param>
        /// <param name="power">Amount of hp that ship can repair</param>
        /// <param name="range">Distance to target ship which can be repaired</param>
        public BasicRepair(uint id, ushort power, ushort range)
            : this(power, range) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicRepair"/> class
        /// </summary>
        /// <param name="power">Amount of hp that ship can repair</param>
        /// <param name="range">Distance to target ship which can be repaired</param>
        public BasicRepair(ushort power, ushort range) : this()
        {
            RepairPower = power;
            RepairRange = range;
        }
    }
}
