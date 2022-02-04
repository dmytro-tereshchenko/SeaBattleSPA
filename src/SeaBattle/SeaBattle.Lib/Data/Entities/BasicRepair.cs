namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic repair equipment for ship
    /// </summary>
    public class BasicRepair : IRepair
    {
        public uint Id { get; set; }

        public ushort RepairPower { get; private set; }

        public ushort RepairRange { get; private set; }

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
        public BasicRepair(ushort power, ushort range)
        {
            RepairPower = power;
            RepairRange = range;
        }
    }
}
