namespace SeaBattle.Lib.Entities
{
    public class BasicRepair : IRepair
    {
        public uint Id { get; set; }

        public ushort RepairPower { get; private set; }

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
