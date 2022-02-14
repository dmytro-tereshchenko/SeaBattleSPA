using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Game (match) between players.
    /// </summary>
    public interface IGame: IEntity
    {
        /// <summary>
        /// Foreign key Id <see cref="IGameField"/>
        /// Field of the game
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GameFieldId { get; set; }

        /// <summary>
        /// Current amount of players which connected to the game
        /// </summary>
        /// <value><see cref="byte"/></value>
        byte CurrentCountPlayers { get; }

        /// <summary>
        /// Max amount of players
        /// </summary>
        /// <value><see cref="byte"/></value>
        byte MaxNumberOfPlayers { get; set; }

        /// <summary>
        /// Player, which needs to move this turn.
        /// </summary>
        /// <value><see cref="string"/></value>
        string CurrentGamePlayerMove { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="IGameState"/>
        /// Current state of game
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GameStateId { get; set; }

        /// <summary>
        /// Winner of the game
        /// </summary>
        /// <value><see cref="string"/></value>
        string Winner { get; set; }

        /// <summary>
        /// Field of the game
        /// </summary>
        /// <value><see cref="IGameField"/></value>
        IGameField GameField { get; set; }

        /// <summary>
        /// Current state of game
        /// </summary>
        /// <value><see cref="GameState"/></value>
        IGameState GameState { get; set; }

        /// <summary>
        /// Collection of fields with initializing data and parameters for every player
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IStartField"/></value>
        ICollection<IStartField> StartFields { get; set; }

        /// <summary>
        /// Collection of players for current game.
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGamePlayer"/></value>
        ICollection<IGamePlayer> Players { get; set; }
    }
}
