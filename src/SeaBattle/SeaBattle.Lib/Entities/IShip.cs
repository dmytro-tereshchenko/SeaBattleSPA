using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface IShip : ICommonShip
    {
        void AddWeapon(IWeapon weapon);
        void AddRepair(IRepair repair);
    }
}
