namespace SeaBattle.Lib.Entities
{
    public class BasicRepair : IRepair
    {
        private uint _id;

        private ushort _repairPower;

        private ushort _repairRange;

        public uint Id
        {
            get => _id;
            private set => _id = value;
        }

        public ushort RepairPower { get => _repairPower; }

        public ushort RepairRange { get => _repairRange; }

        public BasicRepair(uint id, ushort power, ushort range)
        : this(power, range)
        {
            _id = id;
        }

        public BasicRepair(ushort power, ushort range)
        {
            _repairPower = power;
            _repairRange = range;
        }
    }
}
