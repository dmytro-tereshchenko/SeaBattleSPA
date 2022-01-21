using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaBattle.Lib.Entities
{
    public class GameField : IGameField
    {
        protected IGameShip[,] _gameShips;

        public uint Id { get; set; }

        public ushort SizeX { get; private set; }

        public ushort SizeY { get; private set; }

        public GameField(ushort sizeX, ushort sizeY, uint id) : this(sizeX, sizeY) => Id = id;

        public GameField(ushort sizeX, ushort sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;

            _gameShips = new GameShip[SizeX, SizeY];
        }

        /// <summary>
        /// Access by index to game field.
        /// </summary>
        /// <param name="x">Coordinate X of game field, numeration from "1"</param>
        /// <param name="y">Coordinate Y of game field, numeration from "1"</param>
        /// <returns>object of GameShip</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="x"/>, <paramref name="y"/>out of range game field</exception>
        public IGameShip this[ushort x, ushort y]
        {
            get
            {
                if (x < 1 || x > SizeX || y < 1 || y > SizeY)
                {
                    throw new IndexOutOfRangeException(
                        $"[{x},{y}] out of range [1, 1]:[{SizeX},{SizeY}] in {nameof(GameField)}");
                }

                return _gameShips[x - 1, y - 1]; 
            }
            set
            {
                if (x < 1 || x > SizeX || y < 1 || y > SizeY)
                {
                    throw new IndexOutOfRangeException(
                        $"[{x},{y}] out of range [1, 1]:[{SizeX},{SizeY}] in {nameof(GameField)}");
                }

                _gameShips[x - 1, y - 1] = value;
            } 
        }

        /// <summary>
        /// Method for getting status of game field.
        /// </summary>
        /// <param name="teamId">Team's id for getting ships or null for getting ships all teams.</param>
        /// <returns>Collection ships with its own parameters in string format.</returns>
        public ICollection<string> GetFieldWithShips(uint? teamId=null)
        {
            //Dictionary of ships when Key=ship (IGameShip), Value=array of coordinates(X,Y) on field (List<(ushort, ushort)>)
            Dictionary<IGameShip, List<(ushort, ushort)>> ships = new Dictionary<IGameShip, List<(ushort, ushort)>>();

            for (ushort i = 0; i < SizeX; i++)
            {
                for(ushort j = 0; j < SizeY; j++)
                {
                    //filtering by team and empty cell
                    if (_gameShips[i, j] != null && (teamId == null || teamId.Value == _gameShips[i, j].TeamId))
                    {
                        if (!ships.ContainsKey(_gameShips[i, j]))
                        {
                            //create a list of coordinates if this is the first coordinate of the ship
                            ships[_gameShips[i, j]] = new List<(ushort, ushort)>();
                        }

                        ships[_gameShips[i, j]].Add((i, j));
                    }
                }
            }

            (float, float) centerField = (SizeX / 2, SizeY / 2);

            //sorting ships by distance from the center of the field
            var orderedShips = ships
                .OrderBy(s => GetDistanceToCenterField(centerField, GetGeometricCenterOfShip(s.Value)))
                .ToList();

            return orderedShips.Select(s => $"id={s.Key.Id}, teamId={s.Key.TeamId}, " +
                                            $"coords={String.Join(", ", s.Value.Select(coord => $"[{coord.Item1 + 1};{coord.Item2 + 1}]"))}, " + //+1 as outside the entity, numbering starts from "1"
                                            $"type={s.Key.Type.ToString()}, size={s.Key.Size}, hp={s.Key.Hp}/{s.Key.MaxHp}")
                .ToList();
        }

        private (float, float) GetGeometricCenterOfShip(List<(ushort, ushort)> ship)
        {
            float maxX = ship.Max(s => s.Item1);
            float minX = ship.Min(s => s.Item1);

            float maxY = ship.Max(s => s.Item2);
            float minY = ship.Min(s => s.Item2);

            float centerX = (maxX + minX) / 2 + 0.5f;
            float centerY = (maxY + minY) / 2 + 0.5f;

            return (centerX, centerY);
        }

        private float GetDistanceToCenterField((float, float) centerField, (float, float) centerShip) =>
            Convert.ToSingle(Math.Sqrt(Math.Pow(Convert.ToDouble(centerField.Item1 - centerShip.Item1), 2)
                                       + Math.Pow(Convert.ToDouble(centerField.Item2 - centerShip.Item2), 2)));
    }
}
