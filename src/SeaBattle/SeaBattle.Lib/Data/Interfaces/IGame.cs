using System.Collections.Generic;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Game (match) between players.
    /// </summary>
    public interface IGame: IEntity
    {
        /// <summary>
        /// Field of the game
        /// </summary>
        /// <value><see cref="IGameField"/></value>
        IGameField Field { get; set; }

        /// <summary>
        /// Current amount of players which connected to the game
        /// </summary>
        /// <value><see cref="byte"/></value>
        public byte CurrentCountPlayers { get; }

        /// <summary>
        /// Max amount of players
        /// </summary>
        /// <value><see cref="byte"/></value>
        public byte MaxNumberOfPlayers { get; }

        /// <summary>
        /// Player, which needs to move this turn.
        /// </summary>
        /// <value><see cref="IGamePlayer"/></value>
        public IGamePlayer CurrentGamePlayerMove { get; set; }

        /// <summary>
        /// Current state of game
        /// </summary>
        /// <value><see cref="GameState"/></value>
        GameState State { get; set; }

        /// <summary>
        /// Collection of fields with initializing data and parameters for every player
        /// </summary>
        /// <value><see cref="GameState"/></value>
        public ICollection<IStartField> StartFields { get; set; }

        /// <summary>
        /// Collection of players for current game.
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGamePlayer"/></value>
        public ICollection<IGamePlayer> Players { get; set; }
    }
}
