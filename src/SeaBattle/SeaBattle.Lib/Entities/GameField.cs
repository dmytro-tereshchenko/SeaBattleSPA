using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public class GameField : IGameField
    {
        private uint _id;

        private ushort _sizeX;

        private ushort _sizeY;

        protected IGameShip[,] _gameShips;

        public uint Id { get => _id; }

        public ushort SizeX { get => _sizeX; }

        public ushort SizeY { get => _sizeY; }

        public GameField(uint id, ushort sizeX, ushort sizeY)
        {
            _id = id;
            _sizeX = sizeX;
            _sizeY = sizeY;
            _gameShips = new GameShip[_sizeX, _sizeY];
        }

        //numeration from "1"
        public IGameShip this[ushort x, ushort y]
        {
            get
            {
                if (x < 1 || x > _sizeX || y < 1 || y > _sizeY)
                    throw new IndexOutOfRangeException($"[{x},{y}] out of range [1, 1]:[{_sizeX},{_sizeY}] in {nameof(GameField)}");
                return _gameShips[x - 1, y - 1]; 
            }
            set
            {
                if (x < 1 || x > _sizeX || y < 1 || y > _sizeY)
                    throw new IndexOutOfRangeException($"[{x},{y}] out of range [1, 1]:[{_sizeX},{_sizeY}] in {nameof(GameField)}");
                _gameShips[x - 1, y - 1] = value;
            } 
        }

        //null for print all teams
        public ICollection<string> GetFieldWithShips(uint? teamId=null)
        {
            //Dictionary of ships when Key=ship (IGameShip), Value=array of coordinates(X,Y) on field (List<(ushort, ushort)>)
            Dictionary<IGameShip, List<(ushort, ushort)>> ships = new Dictionary<IGameShip, List<(ushort, ushort)>>();
            for (ushort i = 0; i < _sizeX; i++)
            {
                for(ushort j = 0; j < _sizeY; j++)
                {
                    //filtering by team and empty cell
                    if (_gameShips[i, j] != null && (teamId == null || teamId.Value == _gameShips[i, j].TeamId))
                    {
                        if (!ships.ContainsKey(_gameShips[i, j]))
                            //create a list of coordinates if this is the first coordinate of the ship
                            ships[_gameShips[i, j]] = new List<(ushort, ushort)>();
                        ships[_gameShips[i, j]].Add((i, j));
                    }
                }
            }
            (float, float) centerField = (_sizeX / 2, _sizeY / 2);
            //sorting ships by distance from the center of the field
            var orderedShips = ships
                .OrderBy(s => GetDistanceToCenterField(centerField, GetGeometricCenterOfShip(s.Value)))
                .ToList();
            return orderedShips.Select(s => $"id={s.Key.Id}, teamId={s.Key.TeamId}, " +
            $"coords={String.Join(", ", s.Value.Select(coord=>$"[{coord.Item1+1};{coord.Item2+1}]"))}, " +//+1 as outside the entity, numbering starts from "1"
            $"type={s.Key.Type.ToString()}, size={s.Key.Size}, hp={s.Key.Hp}/{s.Key.MaxHp}").ToList();
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
