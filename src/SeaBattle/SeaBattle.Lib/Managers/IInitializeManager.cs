using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

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
        /// Buy ship and add to <see cref="IStartField"/>
        /// </summary>
        /// <param name="players">Collection of players in the game</param>
        /// <param name="gameShip">Game ship</param>
        /// <param name="startField">Start field with initializing data and parameters for the player</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode BuyShip(ICollection<IPlayer> players, IGameShip gameShip, IStartField startField);

        /// <summary>
        /// Sell ship and remove from <see cref="IStartField"/>
        /// </summary>
        /// <param name="players">Collection of players in the game</param>
        /// <param name="gameShip">Game ship</param>
        /// <param name="startField">Start field with initializing data and parameters for the player</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode SellShip(ICollection<IPlayer> players, IGameShip gameShip, IStartField startField);

        /// <summary>
        /// Method for generating a collection of fields with labels for possible placing ships on start field for teams.
        /// </summary>
        /// <param name="gameField">Field of the game</param>
        /// <param name="numberOfPlayers">Amount of players</param>
        /// <returns>Collection of fields with ships - arrays with type bool, where true - placed boat, false - empty</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="numberOfPlayers"/> out of range</exception>
        /// <exception cref="ArgumentNullException">There is no gaming field for the given <paramref name="gameField"/>.</exception>
        ICollection<bool[,]> GenerateStartFields(IGameField gameField, byte numberOfPlayers);

        /// <summary>
        /// Getting a collection of ships, which players can buy by points.
        /// </summary>
        /// <returns>Collection of <see cref="ICommonShip"/>, which players can buy by points</returns>
        ICollection<ICommonShip> GetShips();

        /// <summary>
        /// Getting a collection of repairs, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IRepair"/>, which players can equip on the ship</returns>
        ICollection<IRepair> GetRepairs();

        /// <summary>
        /// Getting a collection of weapons, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IWeapon"/>, which players can equip on the ship</returns>
        ICollection<IWeapon> GetWeapons();

        /// <summary>
        /// Creation and getting a new game ship.
        /// </summary>
        /// <param name="player">Player(user) in game</param>
        /// <param name="ship">Type of <see cref="ICommonShip"/>, which player wants to buy.</param>
        /// <returns><see cref="IGameShip"/> Game ship.</returns>
        IGameShip GetNewShip(IPlayer player, ICommonShip ship);

        /// <summary>
        /// Add weapon to game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which adds a weapon.</param>
        /// <param name="weapon">A weapon (<see cref="IWeapon"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode AddWeapon(IPlayer player, IGameShip gameShip, IWeapon weapon);

        /// <summary>
        /// Add repair to game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which adds a repair.</param>
        /// <param name="repair">A repair (<see cref="IRepair"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode AddRepair(IPlayer player, IGameShip gameShip, IRepair repair);

        /// <summary>
        /// Remove weapon from game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which removes a weapon.</param>
        /// <param name="weapon">Weapon (<see cref="IWeapon"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode RemoveWeapon(IPlayer player, IGameShip gameShip, IWeapon weapon);

        /// <summary>
        /// Remove repair from game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which removes a repair.</param>
        /// <param name="repair">Repair (<see cref="IRepair"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode RemoveRepair(IPlayer player, IGameShip gameShip, IRepair repair);

        /// <summary>
        /// Getting border size of the game field.
        /// </summary>
        /// <returns><see cref="ILimitSize"/></returns>
        ILimitSize GetLimitSizeField();

        /// <summary>
        /// Create and add player to the game
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="playerName">Player's name</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode AddPlayerToGame(IGame game, string playerName);

        /// <summary>
        /// Get start field by player and game. In case absence of starting fields, create them.
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="player">Current player</param>
        /// <returns><see cref="IStartField"/> otherwise null</returns>
        IStartField GetStartField(IGame game, IPlayer player);

        /// <summary>
        /// Create <see cref="IGame"/> by <paramref name="numberOfPlayers"/>
        /// </summary>
        /// <param name="numberOfPlayers">Number of players in the game</param>
        /// <returns><see cref="IGame"/> Created game</returns>
        /// <exception cref="ArgumentOutOfRangeException">A number of teams are out of possible values.</exception>
        IGame CreateGame(byte numberOfPlayers);
    }
}
