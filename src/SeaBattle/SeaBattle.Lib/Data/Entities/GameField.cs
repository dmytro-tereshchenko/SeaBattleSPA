﻿using System;
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
    }
}
