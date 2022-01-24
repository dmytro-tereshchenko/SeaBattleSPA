using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    public interface IGameField : IEntity
    {
        ushort SizeX { get; }

        ushort SizeY { get; }

        //numeration from "1"
        IGameShip this[ushort u, ushort y] { get; set; }

        ICollection<string> GetFieldWithShips(uint? playerId = null);
    }
}
