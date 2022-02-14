using System;
using System.Collections;
using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// A field where ships are allocated
    /// </summary>
    public interface IGameField : IEntity
    {
        /// <summary>
        /// Collection of game field cells with <see cref="IGameShip"/> in cell
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameFieldCell"/></value>
        ICollection<IGameFieldCell> GameFieldCells { get; set; }

        /// <summary>
        /// Size X of game field
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort SizeX { get; set; }

        /// <summary>
        /// Size Y of game field
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort SizeY { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="IGame"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GameId { get; set; }

        /// <summary>
        /// Navigate property <see cref="IGame/>
        /// </summary>
        /// <value><see cref="IGame"/></value>
        IGame Game { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="IStartField"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IStartField"/>.</value>
        ICollection<IStartField> StartFields { get; set; }

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
