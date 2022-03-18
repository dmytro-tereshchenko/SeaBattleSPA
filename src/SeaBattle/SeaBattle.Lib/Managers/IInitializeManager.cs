using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Responses;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for creating and initializing start entities.
    /// </summary>
    public interface IInitializeManager
    {
        /// <summary>
        /// Creating and getting game field.
        /// </summary>
        /// <param name="gameId">Game's Id.</param>
        /// <param name="sizeX">Size X of created game field.</param>
        /// <param name="sizeY">Size Y of created game field.</param>
        /// <returns><see cref="IResponseGameField"/> where Value is <see cref="IGameField"/>, State is <see cref="StateCode"/></returns>
        Task<IResponseGameField> CreateGameField(int gameId, ushort sizeX, ushort sizeY);

        /// <summary>
        /// Method for calculation points which need to purchase ships.
        /// </summary>
        /// <param name="startFieldCells">Collection of FieldCell with placement ships on start game.</param>
        /// <param name="sizeX">Size X of game field</param>
        /// <param name="sizeY">Size Y of game field</param>
        /// <returns>Amount of points</returns>
        int CalculateStartPoints(ICollection<StartFieldCell> startFieldCells, ushort sizeX, ushort sizeY);

        /// <summary>
        /// Getting border size of the game field.
        /// </summary>
        /// <returns><see cref="LimitSize"/></returns>
        LimitSize GetLimitSizeField();

        /// <summary>
        /// Getting max number of players
        /// </summary>
        /// <returns><see cref="byte"/></returns>
        byte GetMaxNumberOfPlayers();

        /// <summary>
        /// Create and add player to the game
        /// </summary>
        /// <param name="gameId">Current game's Id</param>
        /// <param name="playerName">Player's name</param>
        /// <returns><see cref="IResponseGame"/></returns>
        Task<IResponseGame> AddPlayerToGame(int gameId, string playerName);

        /// <summary>
        /// Get start field by player and game. In case absence of starting fields, create them.
        /// </summary>
        /// <param name="gameId">Current game's Id</param>
        /// <param name="gamePlayerName">Current player's name</param>
        /// <returns><see cref="IResponseStartField"/></returns>
        Task<IResponseStartField> GetStartField(int gameId, string gamePlayerName);

        /// <summary>
        /// Create <see cref="IGame"/> by <paramref name="numberOfPlayers"/>
        /// </summary>
        /// <param name="numberOfPlayers">Number of players in the game</param>
        /// <returns><see cref="IGame"/> Created game</returns>
        /// <exception cref="ArgumentOutOfRangeException">A number of teams are out of possible values.</exception>
        Task<IGame> CreateGame(byte numberOfPlayers);

        /// <summary>
        /// End of preparing for game
        /// </summary>
        /// <param name="gameId">Current game's Id</param>
        /// <param name="playerName">Player's name</param>
        /// <returns><see cref="StateCode"/></returns>
        Task<StateCode> ReadyPlayer(int gameId, string gamePlayerName);
    }
}
