﻿using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Possible states of the game.
    /// </summary>
    public interface IGameState : IShortEntity
    {
        /// <summary>
        /// Name of the game state.
        /// </summary>
        /// <value><see cref="string"/></value>
        string Name { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="IGame"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="Game"/>.</value>
        ICollection<Game> Games { get; set; }
    }
}
