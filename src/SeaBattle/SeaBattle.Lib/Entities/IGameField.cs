using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface IGameField : IEntity
    {
        
        ushort SizeX { get; }

        ushort SizeY { get; }

        //numeration from "1"
        IGameShip this[ushort u, ushort y] { get; set; }

        ICollection<string> PrintFieldWithShips(string team = null);

    }
}
