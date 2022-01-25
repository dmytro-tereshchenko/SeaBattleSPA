using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// The manager responsible for the actions and changes of ships during the game, implements <see cref=""/>.
    /// </summary>
    public class ActionManager
    {
        /// <summary>
        /// Get actual game field
        /// </summary>
        /// <param name="player">The player who request game field</param>
        /// <param name="game">Current game</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public IResponseGameField GetGameField(IGamePlayer player, IGame game)
        {
            if (player == null || game == null || game.Field == null)
            {
                return new ResponseGameField(null, StateCode.NullReference);
            }

            if (!game.Players.Contains(player))
            {
                return new ResponseGameField(null, StateCode.InvalidPlayer);
            }

            return new ResponseGameField(game.Field, StateCode.Success);
        }

        /// <summary>
        /// Place ship on game field
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Target ship</param>
        /// <param name="posX">X coordinate of the ship's stern</param>
        /// <param name="posY">Y coordinate of the ship's stern</param>
        /// <param name="direction">The direction of placement ship</param>
        /// <param name="field">Game field</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Used direction not planned by the game</exception>
        public StateCode PutShipOnField(IGamePlayer player, IGameShip ship, ushort posX, ushort posY,
            DirectionOfShipPosition direction, IGameField field)
        {
            if (player == null || ship == null || field == null)
            {
                return StateCode.NullReference;
            }

            if (ship.GamePlayer != player)
            {
                return StateCode.InvalidPlayer;
            }

            byte i = 0;
            bool check = true;
            List<(ushort, ushort)> coordinates = new List<(ushort, ushort)>(ship.Size);

            while (i++ != ship.Size)
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
                        throw new NotImplementedException();
                }
                try
                {
                    check = CheckFreeAreaAroundShip(field, coordinates.Last().Item1, coordinates.Last().Item2, ship);
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
                field[cell.Item1, cell.Item2] = ship;
            }

            return StateCode.Success;
        }

        /// <summary>
        /// Method for checking the free area around the cell for position ship with size equals 1 
        /// </summary>
        /// <param name="field">Field with ships - array with type bool, where <see cref="IGameShip"/> - placed ship, null - empty.</param>
        /// <param name="x">Coordinate X target cell where checks free area. Numeration from "1".</param>
        /// <param name="y">Coordinate Y target cell where checks free area. Numeration from "1".</param>
        /// <param name="ship"></param>
        /// <returns>false - there is another ship around target cell, otherwise true - around area is free.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="x"/>, <paramref name="y"/> is out of range game field.</exception>
        private bool CheckFreeAreaAroundShip(IGameField field, ushort x, ushort y, IGameShip ship)
        {
            ushort sizeX = field.SizeX;
            ushort sizeY = field.SizeY;

            if (x < 0 || x >= sizeX || y < 0 || y > sizeY)
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
                if (x + offsetXY >= 0 && x + offsetXY < sizeX && y + offsetYX >= 0 && y + offsetYX < sizeY &&
                    field[(ushort) (x + offsetXY), (ushort) (y + offsetYX)] != null &&
                    field[(ushort) (x + offsetXY), (ushort) (y + offsetYX)] != ship)
                {
                    return false;
                }

                //Check free cells (absents ship or current ship) on horizontal and vertical from the cell with coordinates x,y
                if (x + offsetX >= 0 && x + offsetX < sizeX && y + offsetY >= 0 && y + offsetY < sizeY &&
                    field[(ushort) (x + offsetX), (ushort) (y + offsetY)] != null &&
                    field[(ushort) (x + offsetX), (ushort) (y + offsetY)] != ship)
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
    }
}
