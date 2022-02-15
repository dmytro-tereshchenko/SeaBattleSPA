using System;
using System.Collections.Generic;
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
       /* /// <summary>
        /// Creating and getting game field.
        /// </summary>
        /// <param name="sizeX">Size X of created game field.</param>
        /// <param name="sizeY">Size Y of created game field.</param>
        /// <returns><see cref="IResponseGameField"/> where Value is <see cref="IGameField"/>, State is <see cref="StateCode"/></returns>
        IResponseGameField CreateGameField(ushort sizeX, ushort sizeY);

        /// <summary>
        /// Method for calculation points which need to purchase ships.
        /// </summary>
        /// <param name="field">Field with placement ships on start game - array with type bool, where true - can placed ship, false - wrong cell.</param>
        /// <returns>Amount of points</returns>
        int CalculateStartPoints(bool[,] field);

        /// <summary>
        /// Method for generating a collection of fields with labels for possible placing ships on start field for teams.
        /// </summary>
        /// <param name="gameField">Field of the game</param>
        /// <param name="numberOfPlayers">Amount of players</param>
        /// <returns>Collection of fields with ships - arrays with type bool, where true - placed boat, false - empty</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="numberOfPlayers"/> out of range</exception>
        ICollection<bool[,]> GenerateStartFields(IGameField gameField, byte numberOfPlayers);

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
        /// <param name="game">Current game</param>
        /// <param name="playerName">Player's name</param>
        /// <returns><see cref="IResponseGamePlayer"/></returns>
        IResponseGamePlayer AddPlayerToGame(IGame game, string playerName);

        /// <summary>
        /// Get start field by player and game. In case absence of starting fields, create them.
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="gamePlayer">Current player</param>
        /// <returns><see cref="IResponseStartField"/></returns>
        IResponseStartField GetStartField(IGame game, IGamePlayer gamePlayer);

        /// <summary>
        /// Create <see cref="IGame"/> by <paramref name="numberOfPlayers"/>
        /// </summary>
        /// <param name="numberOfPlayers">Number of players in the game</param>
        /// <returns><see cref="IGame"/> Created game</returns>
        /// <exception cref="ArgumentOutOfRangeException">A number of teams are out of possible values.</exception>
        IGame CreateGame(byte numberOfPlayers);*/
    }
}
