using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface IRepair
    {
        uint Id { get; }
        ushort RepairPower { get; }
        ushort RepairRange { get; }
    }
}
