using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    public class GameFieldActionUtility : IGameFieldActionUtility
    {
        public StateCode PutShipOnField(string playerName, IGameShip ship, ushort posX, ushort posY,
            DirectionOfShipPosition direction, IGameField field)
        {
            if (!ship.GamePlayer.Name.Equals(playerName))
            {
                return StateCode.InvalidPlayer;
            }

            bool check = true;
            ICollection<(ushort, ushort)> coordinates = GetCoordinatesShipByPosition(ship.Size, posX, posY, direction);

            foreach (var coordinate in coordinates)
            {
                try
                {
                    check = CheckFreeAreaAroundShip(field, coordinate.Item1, coordinate.Item2, ship);
                }
                catch (ArgumentOutOfRangeException)
                {
                    check = false;
                }

                if (!check)
                {
                    return StateCode.InvalidPositionShip;
                }
            }

            foreach (var cell in coordinates)
            {
                field[cell.Item1, cell.Item2] = ship as GameShip;
            }

            field.GameFieldCells.FirstOrDefault(c => c.X == posX && c.Y == posY).Stern = true;

            return StateCode.Success;
        }

        /// <summary>
        /// Prognosis coordinates of the ship by position stern and direction
        /// </summary>
        /// <param name="shipSize">Size of <see cref="IGameShip"/></param>
        /// <param name="posX">Coordinate X of position</param>
        /// <param name="posY">Coordinate Y of position</param>
        /// <param name="direction">The direction of allocating of the ship relatively by stern</param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public ICollection<(ushort, ushort)> GetCoordinatesShipByPosition(byte shipSize, ushort posX, ushort posY,
            DirectionOfShipPosition direction)
        {
            byte i = 0;
            List<(ushort, ushort)> coordinates = new List<(ushort, ushort)>(shipSize);

            while (i++ != shipSize)
            {
                switch (direction)
                {
                    case DirectionOfShipPosition.XDec:
                        coordinates.Add(new(posX--, posY));
                        break;
                    case DirectionOfShipPosition.XInc:
                        coordinates.Add(new(posX++, posY));
                        break;
                    case DirectionOfShipPosition.YDec:
                        coordinates.Add(new(posX, posY--));
                        break;
                    case DirectionOfShipPosition.YInc:
                        coordinates.Add(new(posX, posY++));
                        break;
                    default:
                        throw new InvalidEnumArgumentException("Wrong direction of ship");
                }
            }

            return coordinates;
        }

        public bool CheckFreeAreaAroundShip(IGameField field, ushort x, ushort y, IGameShip ship)
        {
            ushort sizeX = field.SizeX;
            ushort sizeY = field.SizeY;

            if (x < 1 || x > sizeX || y < 1 || y > sizeY)
            {
                throw new ArgumentOutOfRangeException();
            }

            short offsetX = -1;
            short offsetY = 0;
            short offsetXY;
            short offsetYX;

            for (ushort i = 0; i < 4; i++)
            {
                //Check free cells (absents ship or current ship) on diagonal from the cell with coordinates x,y
                offsetXY = Convert.ToInt16(Math.Pow(-1, i / 2));
                offsetYX = Convert.ToInt16(Math.Pow(-1, (i + 1) / 2));
                if (x + offsetXY > 0 && x + offsetXY <= sizeX && y + offsetYX > 0 && y + offsetYX <= sizeY &&
                    field[(ushort)(x + offsetXY), (ushort)(y + offsetYX)] is not null &&
                    field[(ushort)(x + offsetXY), (ushort)(y + offsetYX)] != ship)
                {
                    return false;
                }

                //Check free cells (absents ship or current ship) on horizontal and vertical from the cell with coordinates x,y
                if (x + offsetX > 0 && x + offsetX <= sizeX && y + offsetY > 0 && y + offsetY <= sizeY &&
                    field[(ushort)(x + offsetX), (ushort)(y + offsetY)] is not null &&
                    field[(ushort)(x + offsetX), (ushort)(y + offsetY)] != ship)
                {
                    return false;
                }

                //Change offset for check horizontal and vertical sides.
                offsetX += Convert.ToInt16(Math.Pow(-1, i / 2));
                offsetY += Convert.ToInt16(Math.Pow(-1, (i + 1) / 2));
            }

            //If we don't find a ship that area is free.
            return true;
        }

        public ICollection<(ushort, ushort)> GetShipCoordinates(IGameShip ship, IGameField field)
        {
            ICollection<(ushort, ushort)> coordinates = new List<(ushort, ushort)>(ship.Size);

            for (ushort i = 1; i <= field.SizeX; i++)
            {
                for (ushort j = 1; j <= field.SizeY; j++)
                {
                    if (field[i, j] == ship)
                    {
                        //add first founded cell of the ship
                        coordinates.Add((i, j));

                        //add other vertical cells of the ship
                        ushort tempCoordinate = (ushort)(i + 1);
                        while (tempCoordinate <= field.SizeX && field[tempCoordinate, j] == ship)
                        {
                            coordinates.Add((tempCoordinate, j));
                            tempCoordinate++;
                        }

                        //add other horizontal cells of the ship
                        tempCoordinate = (ushort)(j + 1);
                        while (tempCoordinate <= field.SizeY && field[i, tempCoordinate] == ship)
                        {
                            coordinates.Add((i, tempCoordinate));
                            tempCoordinate++;
                        }

                        return coordinates;
                    }
                }
            }

            return coordinates;
        }

        public IDictionary<IGameShip, ICollection<(ushort, ushort)>> GetAllShipsCoordinates(IGameField field,
            string playerName = null)
        {
            //Dictionary of ships when Key=ship (IGameShip), Value=array of coordinates(X,Y) on field (List<(ushort, ushort)>)
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> ships = new Dictionary<IGameShip, ICollection<(ushort, ushort)>>();

            for (ushort i = 1; i <= field.SizeX; i++)
            {
                for (ushort j = 1; j <= field.SizeY; j++)
                {
                    //filtering by team and empty cell
                    if (field[i, j] is not null && (playerName is null || playerName.Equals(field[i, j].GamePlayer.Name)))
                    {
                        if (!ships.ContainsKey(field[i, j]))
                        {
                            //create a list of coordinates if this is the first coordinate of the ship
                            ships[field[i, j]] = new List<(ushort, ushort)>();
                        }

                        ships[field[i, j]].Add((i, j));
                    }
                }
            }

            return ships;
        }

        public (float, float) GetGeometricCenterOfShip(ICollection<(ushort, ushort)> ship)
        {
            float maxX = ship.Max(s => s.Item1);
            float minX = ship.Min(s => s.Item1);

            float maxY = ship.Max(s => s.Item2);
            float minY = ship.Min(s => s.Item2);

            float centerX = (maxX + minX) / 2 - 0.5f;
            float centerY = (maxY + minY) / 2 - 0.5f;

            return (centerX, centerY);
        }

        public float GetDistanceBetween2Points((float, float) point1, (float, float) point2) =>
            Convert.ToSingle(Math.Sqrt(Math.Pow(Convert.ToDouble(point1.Item1 - point2.Item1), 2)
                                       + Math.Pow(Convert.ToDouble(point1.Item2 - point2.Item2), 2)));

        public StateCode RemoveShipFromField(IGameShip ship, IGameField field)
        {
            foreach (var shipsCell in GetShipCoordinates(ship, field))
            {
                field[shipsCell.Item1, shipsCell.Item2] = null;
            }

            return StateCode.Success;
        }
    }
}
