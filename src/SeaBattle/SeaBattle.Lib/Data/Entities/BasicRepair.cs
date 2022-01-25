namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic repair equipment for ship
    /// </summary>
    public class BasicRepair : IRepair
    {
        /// <summary>
        /// Id Entity
        /// </summary>
        /// <value><see cref="uint"/></value>
        public uint Id { get; set; }

        /// <summary>
        /// Amount of hp that ship can be repaired.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort RepairPower { get; private set; }

        /// <summary>
        /// Distance to target ship which can be repaired.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort RepairRange { get; private set; }

        public BasicRepair(uint id, ushort power, ushort range)
            : this(power, range) => Id = id;

        public BasicRepair(ushort power, ushort range)
        {
            RepairPower = power;
            RepairRange = range;
        }
    }
}
