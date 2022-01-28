using System.Collections.Generic;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Responses;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for game flow of "Sea battle"
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Create <see cref="IGame"/>
        /// </summary>
        /// <param name="numberOfPlayers">Number of players in <see cref="IGame"/></param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode CreateGame(byte numberOfPlayers);

        /// <summary>
        /// Create <see cref="IGameField"/>
        /// </summary>
        /// <param name="sizeX">SizeX of <see cref="IGameField"/></param>
        /// <param name="sizeY">SizeY of <see cref="IGameField"/></param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode CreateGameField(ushort sizeX, ushort sizeY);

        /// <summary>
        /// Add player to the game
        /// </summary>
        /// <param name="playerName">Player's name</param>
        /// <returns><see cref="IResponseGamePlayer"/></returns>
        IResponseGamePlayer AddGamePlayer(string playerName);

        /// <summary>
        /// Get start field by player and game. In case absence of starting fields, create them.
        /// </summary>
        /// <param name="player">The player who request start field</param>
        /// <returns><see cref="IResponseStartField"/></returns>
        IResponseStartField GetStartField(IGamePlayer player);

        /// <summary>
        /// Get actual game field
        /// </summary>
        /// <param name="player">The player who request game field</param>
        /// <returns><see cref="IResponseGameField"/></returns>
        IResponseGameField GetGameField(IGamePlayer player);

        /// <summary>
        /// Getting a collection of ships, which players can buy by points.
        /// </summary>
        /// <returns>Collection of <see cref="ICommonShip"/>, which players can buy by points</returns>
        ICollection<ICommonShip> GetShips();

        /// <summary>
        /// Getting a collection of weapons, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IWeapon"/>, which players can equip on the ship</returns>
        ICollection<IWeapon> GetWeapons();

        /// <summary>
        /// Getting a collection of repairs, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IRepair"/>, which players can equip on the ship</returns>
        ICollection<IRepair> GetRepairs();

        /// <summary>
        /// Get sorted collection of the ship by distance to the center of the field.
        /// </summary>
        /// <param name="player">Current player for filtering <see cref="IGameShip"/>, if null - get all ships without filter.</param>
        /// <returns><see cref="IDictionary{TKey,TValue}"/> whose generic key argument is <see cref="IGameShip"/>, generic type argument
        /// is <see cref="ICollection{T}"/> whose generic type argument is (<see cref="ushort"/>, <see cref="ushort"/>) coordinates (X,Y)</returns>
        IDictionary<IGameShip, ICollection<(ushort, ushort)>> GetSortedShipsFromField(IGamePlayer player);

        /// <summary>
        /// Get player who win the game;
        /// </summary>
        /// <returns><see cref="IGamePlayer"/>, otherwise null</returns>
        IGamePlayer GetResultGame();

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
        /// Buy ship and save in <see cref="IStartField"/>
        /// </summary>
        /// <param name="player">The player who request buying ship</param>
        /// <param name="ship">The base ship that is buying</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode BuyShip(IGamePlayer player, ICommonShip ship);

        /// <summary>
        /// Sell ship and save in <see cref="IStartField"/>
        /// </summary>
        /// <param name="player">The player who request selling ship</param>
        /// <param name="ship">The ship that is selling</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode SellShip(IGamePlayer player, IGameShip ship);

        /// <summary>
        /// Add weapon to game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Game ship which adds a weapon.</param>
        /// <param name="weapon">A weapon (<see cref="IWeapon"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode AddWeapon(IGamePlayer player, IGameShip ship, IWeapon weapon);

        /// <summary>
        /// Add repair to game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Game ship which adds a repair.</param>
        /// <param name="repair">A repair (<see cref="IRepair"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode AddRepair(IGamePlayer player, IGameShip ship, IRepair repair);

        /// <summary>
        /// Remove weapon from game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Game ship which removes a weapon.</param>
        /// <param name="weapon">Weapon (<see cref="IWeapon"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode RemoveWeapon(IGamePlayer player, IGameShip ship, IWeapon weapon);

        /// <summary>
        /// Remove repair from game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Game ship which removes a repair.</param>
        /// <param name="repair">Repair (<see cref="IRepair"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode RemoveRepair(IGamePlayer player, IGameShip ship, IRepair repair);

        /// <summary>
        /// Place ship on game field
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Target ship</param>
        /// <param name="posX">X coordinate of the ship's stern</param>
        /// <param name="posY">Y coordinate of the ship's stern</param>
        /// <param name="direction">The direction of placement ship</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode PutShipOnField(IGamePlayer player, IGameShip ship, ushort posX, ushort posY,
            DirectionOfShipPosition direction);

        /// <summary>
        /// Remove <see cref="IGameShip"/> from <see cref="IGameField"/> to collection in <see cref="IStartField.Ships"/>
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="posX">X coordinate of removed ship</param>
        /// <param name="posY">Y coordinate of removed ship</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode RemoveShipFromField(IGamePlayer player, ushort posX, ushort posY);

        /// <summary>
        /// Set state ready for player (finished buying ships and placing them on the game field)
        /// </summary>
        /// <param name="player">Current player</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode ReadyPlayer(IGamePlayer player);

        /// <summary>
        /// Get <see cref="ICollection{T}"/> of <see cref="IGameShip"/> on distance of action.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="action">Type of possible actions (<see cref="ActionType.Attack"/>, <see cref="ActionType.Repair"/>)</param>
        /// <returns><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"/>, otherwise default</returns>
        ICollection<IGameShip> GetVisibletargetsforShip(IGamePlayer player, IGameShip ship, ActionType action);

        /// <summary>
        /// Attack cell by ship
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="posX">X coordinate of target cell</param>
        /// <param name="posY">Y coordinate of target cell</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode AttackShip(IGamePlayer player, IGameShip ship, ushort posX, ushort posY);

        /// <summary>
        /// Repair ship by cell
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="posX">X coordinate of target cell</param>
        /// <param name="posY">Y coordinate of target cell</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode RepairShip(IGamePlayer player, IGameShip ship, ushort posX, ushort posY);

        /// <summary>
        /// Repair all friendly ship on distance
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode RepairAllShip(IGamePlayer player, IGameShip ship);
    }
}
