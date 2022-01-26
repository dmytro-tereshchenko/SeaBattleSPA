﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// <returns><see cref="StateCode"/> result of operation</returns>
        /// <exception cref="InvalidEnumArgumentException">Used direction not planned by the game</exception>
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
                        throw new InvalidEnumArgumentException();
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
        /// <param name="ship">Current ship</param>
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

        /// <summary>
        /// Remove <see cref="IGameShip"/> from <see cref="IGameField"/> to collection in <see cref="IStartField.Ships"/>
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="tPosX">X coordinate of removed ship</param>
        /// <param name="tPosY">Y coordinate of removed ship</param>
        /// <param name="startField">Start field with game field and collection of unused ships</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode RemoveShipFromFieldToStartField(IPlayer player, ushort tPosX, ushort tPosY,
            IStartField startField)
        {
            if (player == null || startField == null)
            {
                return StateCode.NullReference;
            }

            IGameShip ship = startField.GameField[tPosX, tPosY];

            if (ship == null)
            {
                return StateCode.AbsentOfShip;
            }

            if (ship.GamePlayer != player || startField.GamePlayer != player)
            {
                return StateCode.InvalidPlayer;
            }

            startField.Ships.Add(ship);
            startField.GameField[tPosX, tPosY] = null;

            return StateCode.Success;
        }

        /// <summary>
        /// Put <see cref="IGameShip"/> from collection of <see cref="IStartField.Ships"/> to <see cref="IGameField"/>
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="tPosX">X coordinate of removed ship</param>
        /// <param name="tPosY">Y coordinate of removed ship</param>
        /// <param name="direction">The direction of placement ship</param>
        /// <param name="startField">Start field with game field and collection of unused ships</param>
        /// <param name="ship">Current ship</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode PutShipFromStartFieldToField(IGamePlayer player, ushort tPosX, ushort tPosY,
            DirectionOfShipPosition direction, IStartField startField, IGameShip ship)
        {
            if (player == null || startField == null || ship == null)
            {
                return StateCode.NullReference;
            }

            if (!startField.Ships.Contains(ship))
            {
                return StateCode.InvalidShip;
            }

            if (ship.GamePlayer != player || startField.GamePlayer != player)
            {
                return StateCode.InvalidPlayer;
            }

            StateCode result;
            try
            {
                result = PutShipOnField(player, ship, tPosX, tPosY, direction, startField.GameField);
            }
            catch (InvalidEnumArgumentException)
            {
                result = StateCode.InvalidOperation;
            }

            if (result == StateCode.Success)
            {
                startField.Ships.Remove(ship);
            }

            return result;
        }

        /// <summary>
        /// Move <see cref="IGameShip"/> from one position to another on <see cref="IGameField"/>
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="tPosX">X coordinate of removed ship</param>
        /// <param name="tPosY">Y coordinate of removed ship</param>
        /// <param name="direction">The direction of placement ship</param>
        /// <param name="field">Game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode MoveShip(IGamePlayer player, IGameShip ship, ushort tPosX, ushort tPosY,
            DirectionOfShipPosition direction, IGameField field)
        {
            if (player == null || field == null || ship == null)
            {
                return StateCode.NullReference;
            }

            if (ship.GamePlayer != player)
            {
                return StateCode.InvalidPlayer;
            }

            //Ship's coordinates on game field
            List<(ushort, ushort)> locOfShip = new List<(ushort, ushort)>(ship.Size);

            for (ushort i = 1; i <= field.SizeX; i++)
            {
                for (ushort j = 1; j <= field.SizeY; j++)
                {
                    if (field[i, j] == ship)
                    {
                        locOfShip.Add((i,j));
                    }
                }
            }

            StateCode result;

            try
            {
                result = PutShipOnField(player, ship, tPosX, tPosY, direction, field);
            }
            catch (InvalidEnumArgumentException)
            {
                result = StateCode.InvalidOperation;
            }

            if (result == StateCode.Success)
            {
                //clear previous position of ship
                foreach (var loc in locOfShip)
                {
                    field[loc.Item1, loc.Item2] = null;
                }
            }

            return result;
        }

        /// <summary>
        /// Get <see cref="ICollection{T}"/> of <see cref="IGameShip"/> on distance of action.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="field">Game field</param>
        /// <param name="action">Type of possible actions (<see cref="TypeOfAction.Attack"/>, <see cref="TypeOfAction.Repair"/>)</param>
        /// <returns><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"/></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentException">Wrong player</exception>
        /// <exception cref="InvalidEnumArgumentException">Used action not planned by the game</exception>
        public ICollection<IGameShip> GetVisibleTargetsForShip(IGamePlayer player, IGameShip ship, IGameField field,
            TypeOfAction action)
        {
            if (player == null || field == null || ship == null)
            {
                throw new NullReferenceException();
            }

            if (ship.GamePlayer != player)
            {
                throw new ArgumentException();
            }

            //Search coordinates of ships
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> ships = GetCoordinateOfShip(field, player);

            //Coordinates of geometric center for current ship
            (float, float) centerOfCurrentShip = GetGeometricCenterOfShip(ships[ship]);

            ushort distance = action switch
            {
                TypeOfAction.Attack => ship.AttackRange,
                TypeOfAction.Repair => ship.RepairRange,
                _ => throw new InvalidEnumArgumentException()
            };

            return ships
                .Where(s => GetDistanceBetween2Points(centerOfCurrentShip, GetGeometricCenterOfShip(s.Value)) <=
                            distance)
                .Select(s => s.Key)
                .ToList();
        }

        /// <summary>
        /// Get in <see cref="string"/> format sorted collection of the ship by distance to the center of the field.
        /// </summary>
        /// <param name="field">Game field</param>
        /// <param name="player">Current player for filtering <see cref="IGameShip"/>, if null - get all ships without filter.</param>
        /// <returns><see cref="ICollection{T}"/> whose generic type argument is <see cref="string"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public ICollection<string> GetFieldWithShips(IGameField field, IGamePlayer player = null)
        {
            if (field == null)
            {
                throw new ArgumentNullException();
            }

            //Search coordinates of ships
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> ships = GetCoordinateOfShip(field, player);

            (float, float) centerField = (field.SizeX / 2, field.SizeY / 2);

            //sorting ships by distance from the center of the field
            var orderedShips = ships
                .OrderBy(s => GetDistanceBetween2Points(centerField, GetGeometricCenterOfShip(s.Value)))
                .ToList();

            return orderedShips.Select(s => $"id={s.Key.Id}, playerId={s.Key.GamePlayer.Id}, " +
                                            $"coords={String.Join(", ", s.Value.Select(coord => $"[{coord.Item1};{coord.Item2}]"))}, " + //+1 as outside the entity, numbering starts from "1"
                                            $"type={s.Key.Type.ToString()}, size={s.Key.Size}, hp={s.Key.Hp}/{s.Key.MaxHp}")
                .ToList();
        }

        /// <summary>
        /// Get coordinates of ships  
        /// </summary>
        /// <param name="field">Game field</param>
        /// <param name="player">Current player for filtering <see cref="IGameShip"/>, if null - get all ships without filter.</param>
        /// <returns><see cref="IDictionary{TKey,TValue}"/> whose generic key argument is <see cref="IGameShip"/>, generic type argument
        /// is <see cref="ICollection{T}"/> whose generic type argument is (<see cref="ushort"/>, <see cref="ushort"/>) coordinates (X,Y)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private IDictionary<IGameShip, ICollection<(ushort, ushort)>> GetCoordinateOfShip(IGameField field,
            IGamePlayer player = null)
        {
            if (field == null)
            {
                throw new ArgumentNullException();
            }

            //Dictionary of ships when Key=ship (IGameShip), Value=array of coordinates(X,Y) on field (List<(ushort, ushort)>)
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> ships = new Dictionary<IGameShip, ICollection<(ushort, ushort)>>();

            for (ushort i = 1; i <= field.SizeX; i++)
            {
                for (ushort j = 1; j <= field.SizeY; j++)
                {
                    //filtering by team and empty cell
                    if (field[i, j] != null && (player == null || player == field[i, j].GamePlayer))
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

        /// <summary>
        /// Calculate and get coordinates of the geometric center of the ship
        /// </summary>
        /// <param name="ship">Coordinates of the cells where the ship is allocated.</param>
        /// <returns>(<see cref="float"/>, <see cref="float"/>) Coordinates of the geometric center of the ship (x, y), when numeration starts from "1"</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private (float, float) GetGeometricCenterOfShip(ICollection<(ushort, ushort)> ship)
        {
            if (ship == null)
            {
                throw new ArgumentNullException();
            }
            
            float maxX = ship.Max(s => s.Item1);
            float minX = ship.Min(s => s.Item1);

            float maxY = ship.Max(s => s.Item2);
            float minY = ship.Min(s => s.Item2);

            float centerX = (maxX + minX) / 2 - 0.5f;
            float centerY = (maxY + minY) / 2 - 0.5f;

            return (centerX, centerY);
        }

        /// <summary>
        /// Calculate and get distance between 2 points on game field.
        /// </summary>
        /// <param name="point1">(<see cref="float"/>, <see cref="float"/>) coordinates of first point on game field (x,y)</param>
        /// <param name="point2">(<see cref="float"/>, <see cref="float"/>) coordinates of second point on game field (x,y)</param>
        /// <returns><see cref="float"/> distance between 2 points on game field</returns>
        private float GetDistanceBetween2Points((float, float) point1, (float, float) point2) =>
            Convert.ToSingle(Math.Sqrt(Math.Pow(Convert.ToDouble(point1.Item1 - point2.Item1), 2)
                                       + Math.Pow(Convert.ToDouble(point1.Item2 - point2.Item2), 2)));
    }
}