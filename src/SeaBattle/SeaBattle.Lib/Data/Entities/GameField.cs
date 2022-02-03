using System;
using System.Collections;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="GameField"/> class
        /// </summary>
        /// <param name="sizeX">Size X of game field</param>
        /// <param name="sizeY">Size Y of game field</param>
        /// <param name="id">Id of game field</param>
        public GameField(ushort sizeX, ushort sizeY, uint id) : this(sizeX, sizeY) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameField"/> class
        /// </summary>
        /// <param name="sizeX">Size X of game field</param>
        /// <param name="sizeY">Size Y of game field</param>
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

        public override bool Equals(object? obj)
        {
            if (obj is not GameField)
            {
                return false;
            }
            GameField ob = obj as GameField;

            if (this.Id.Equals(ob.Id) && this.SizeX.Equals(ob.SizeX) && this.SizeY.Equals(ob.SizeY))
            {
                for (int i = 0; i < SizeX; i++)
                {
                    for (int j = 0; j < SizeY; j++)
                    {
                        if ((ob._gameShips[i, j] == null && this._gameShips[i, j] != null) ||
                            (ob._gameShips[i, j] != null && this._gameShips[i, j] == null) ||
                            (ob._gameShips[i, j] != null && this._gameShips[i, j] != null &&
                             !ob._gameShips[i, j].Equals(this._gameShips[i, j])))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator GetEnumerator() => _gameShips.GetEnumerator();

    }
}
