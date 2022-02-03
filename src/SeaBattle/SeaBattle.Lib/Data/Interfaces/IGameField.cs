using System;
using System.Collections;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// A field where ships are allocated
    /// </summary>
    public interface IGameField : IEntity
    {
        /// <summary>
        /// Size X of game field
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort SizeX { get; }

        /// <summary>
        /// Size Y of game field
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort SizeY { get; }

        /// <summary>
        /// Access by index to game field.
        /// </summary>
        /// <param name="x">Coordinate X of game field, numeration from "1"</param>
        /// <param name="y">Coordinate Y of game field, numeration from "1"</param>
        /// <returns>object of GameShip</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="x"/>, <paramref name="y"/>out of range game field</exception>
        IGameShip this[ushort u, ushort y] { get; set; }

        /// <summary>
        /// Get Enumerator of collection
        /// </summary>
        /// <returns><see cref="IEnumerator"/></returns>
        IEnumerator GetEnumerator();
    }
}
