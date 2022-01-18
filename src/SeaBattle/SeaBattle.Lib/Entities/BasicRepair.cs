using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public class BasicRepair : IRepair, IEntity
    {
        private uint _id;

        private ushort _repairPower;

        private ushort _repairRange;
        public uint Id { get => _id; }
        public ushort RepairPower { get => _repairPower; }
        public ushort RepairRange { get => _repairRange; }
        public BasicRepair(uint id, ushort power, ushort range)
        {
            this._id = id;
            this._repairPower = power;
            this._repairRange = range;
        }
    }
}
