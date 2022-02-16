using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Game (match) between players.
    /// </summary>
    public interface IGame: IEntity
    {
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
        string? CurrentGamePlayerMove { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="GameState"/>
        /// Current state of game
        /// </summary>
        /// <value><see cref="int"/></value>
        int GameStateId { get; set; }

        /// <summary>
        /// Winner of the game
        /// </summary>
        /// <value><see cref="string"/></value>
        string Winner { get; set; }

        /// <summary>
        /// Field of the game
        /// </summary>
        /// <value><see cref="GameField"/></value>
        GameField GameField { get; set; }

        /// <summary>
        /// Current state of game
        /// </summary>
        /// <value><see cref="GameState"/></value>
        GameState GameState { get; set; }

        /// <summary>
        /// Collection of fields with initializing data and parameters for every player
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="StartField"/></value>
        ICollection<StartField> StartFields { get; set; }

        /// <summary>
        /// Collection of players for current game.
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="GamePlayer"/></value>
        ICollection<GamePlayer> GamePlayers { get; set; }
    }
}
