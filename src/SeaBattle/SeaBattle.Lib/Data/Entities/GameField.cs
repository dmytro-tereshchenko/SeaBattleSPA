using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// A field where ships are allocated
    /// </summary>
    public class GameField : IGameField
    {
        /// <summary>
        /// Array of game field with <see cref="IGameShip"/> in cell
        /// </summary>
        /// <value><see cref="IGameShip"/>[,] with null in the cell when the ship is absent</value>
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

        public ICollection<string> GetFieldWithShips(uint? playerId=null)
        {
            //Dictionary of ships when Key=ship (IGameShip), Value=array of coordinates(X,Y) on field (List<(ushort, ushort)>)
            Dictionary<IGameShip, List<(ushort, ushort)>> ships = new Dictionary<IGameShip, List<(ushort, ushort)>>();

            for (ushort i = 0; i < SizeX; i++)
            {
                for(ushort j = 0; j < SizeY; j++)
                {
                    //filtering by team and empty cell
                    if (_gameShips[i, j] != null && (playerId == null || playerId.Value == _gameShips[i, j].Player.Id))
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
                .OrderBy(s => GetDistanceBetween2Points(centerField, GetGeometricCenterOfShip(s.Value)))
                .ToList();

            return orderedShips.Select(s => $"id={s.Key.Id}, playerId={s.Key.Player.Id}, " +
                                            $"coords={String.Join(", ", s.Value.Select(coord => $"[{coord.Item1 + 1};{coord.Item2 + 1}]"))}, " + //+1 as outside the entity, numbering starts from "1"
                                            $"type={s.Key.Type.ToString()}, size={s.Key.Size}, hp={s.Key.Hp}/{s.Key.MaxHp}")
                .ToList();
        }

        /// <summary>
        /// Calculate and get coordinates of the geometric center of the ship
        /// </summary>
        /// <param name="ship">Coordinates of the cells where the ship is allocated.</param>
        /// <returns>(<see cref="float"/>, <see cref="float"/>) Coordinates of the geometric center of the ship (x, y)</returns>
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
