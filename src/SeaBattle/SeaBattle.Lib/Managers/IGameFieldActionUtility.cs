﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    public interface IGameFieldActionUtility
    {
        /// <summary>
        /// Place ship on game field
        /// </summary>
        /// <param name="playerName">Current player's name</param>
        /// <param name="ship">Target ship</param>
        /// <param name="posX">X coordinate of the ship's stern</param>
        /// <param name="posY">Y coordinate of the ship's stern</param>
        /// <param name="direction">The direction of placement ship</param>
        /// <param name="field">Game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        /// <exception cref="InvalidEnumArgumentException">Used direction not planned by the game</exception>
        StateCode PutShipOnField(string playerName, IGameShip ship, ushort posX, ushort posY,
            DirectionOfShipPosition direction, IGameField field);

        /// <summary>
        /// Prognosis coordinates of the ship by position stern and direction
        /// </summary>
        /// <param name="shipSize">Size of <see cref="IGameShip"/></param>
        /// <param name="posX">Coordinate X of position</param>
        /// <param name="posY">Coordinate Y of position</param>
        /// <param name="direction">The direction of allocating of the ship relatively by stern</param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        ICollection<(ushort, ushort)> GetCoordinatesShipByPosition(byte shipSize, ushort posX, ushort posY,
            DirectionOfShipPosition direction);

        /// <summary>
        /// Method for checking the free area around the cell for position ship with size equals 1 
        /// </summary>
        /// <param name="field">Field with ships - array with type bool, where <see cref="IGameShip"/> - placed ship, null - empty.</param>
        /// <param name="x">Coordinate X target cell where checks free area. Numeration from "1".</param>
        /// <param name="y">Coordinate Y target cell where checks free area. Numeration from "1".</param>
        /// <param name="ship">Current ship</param>
        /// <returns>false - there is another ship around target cell, otherwise true - around area is free.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="x"/>, <paramref name="y"/> is out of range game field.</exception>
        bool CheckFreeAreaAroundShip(IGameField field, ushort x, ushort y, IGameShip ship);

        /// <summary>
        /// Get coordinates of cells of the ship
        /// </summary>
        /// <param name="ship">Current ship</param>
        /// <param name="field">Game field</param>
        /// <returns><see cref="ICollection{T}"/> whose generic type argument is (<see cref="ushort"/>, <see cref="ushort"/>) coordinates (X, Y)</returns>
        ICollection<(ushort, ushort)> GetShipCoordinates(IGameShip ship, IGameField field);

        /// <summary>
        /// Get coordinates of ships  
        /// </summary>
        /// <param name="field">Game field</param>
        /// <param name="playerName">Current player's name for filtering <see cref="IGameShip"/>, if null - get all ships without filter.</param>
        /// <returns><see cref="IDictionary{TKey,TValue}"/> whose generic key argument is <see cref="IGameShip"/>, generic type argument
        /// is <see cref="ICollection{T}"/> whose generic type argument is (<see cref="ushort"/>, <see cref="ushort"/>) coordinates (X,Y)</returns>
        IDictionary<IGameShip, ICollection<(ushort, ushort)>> GetAllShipsCoordinates(IGameField field,
            string playerName = null);

        /// <summary>
        /// Calculate and get coordinates of the geometric center of the ship
        /// </summary>
        /// <param name="ship">Coordinates of the cells where the ship is allocated.</param>
        /// <returns>(<see cref="float"/>, <see cref="float"/>) Coordinates of the geometric center of the ship (x, y), when numeration starts from "1"</returns>
        (float, float) GetGeometricCenterOfShip(ICollection<(ushort, ushort)> ship);

        /// <summary>
        /// Calculate and get distance between 2 points on game field.
        /// </summary>
        /// <param name="point1">(<see cref="float"/>, <see cref="float"/>) coordinates of first point on game field (x,y)</param>
        /// <param name="point2">(<see cref="float"/>, <see cref="float"/>) coordinates of second point on game field (x,y)</param>
        /// <returns><see cref="float"/> distance between 2 points on game field</returns>
        float GetDistanceBetween2Points((float, float) point1, (float, float) point2);

        /// <summary>
        /// Remove <see cref="IGameShip"/> from <see cref="IGameField"/>
        /// </summary>
        /// <param name="ship">Current ship</param>
        /// <param name="field">Game field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode RemoveShipFromField(IGameShip ship, IGameField field);
    }
}
