using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface IGameShip : ICommonShip
    {
        
        IShip Ship { get; }

        //current hp
        ushort Hp { get; set; }

        uint TeamId { get; }

        int Points { get; }
    }
}
