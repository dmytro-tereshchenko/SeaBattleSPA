using System.Collections.Generic;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic weapon equipment for ship
    /// </summary>
    public class BasicWeapon : IWeapon
    {
        public uint Id { get; set; }

        public ushort Damage { get; set; }

        public ushort AttackRange { get; set; }

        [JsonIgnore]
        public ICollection<IGameShip> GameShips { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasicWeapon()
        {
            GameShips = new List<IGameShip>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicWeapon"/> class
        /// </summary>
        /// <param name="id">Id of weapon</param>
        /// <param name="damage">Amount of hp that target ship can be damaged</param>
        /// <param name="aRange">Distance to target ship which can be damaged</param>
        public BasicWeapon(uint id, ushort damage, ushort aRange)
            : this(damage, aRange) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicWeapon"/> class
        /// </summary>
        /// <param name="damage">Amount of hp that target ship can be damaged</param>
        /// <param name="aRange">Distance to target ship which can be damaged</param>
        public BasicWeapon(ushort damage, ushort aRange) : this()
        {
            Damage = damage;
            AttackRange = aRange;
        }
    }
}
