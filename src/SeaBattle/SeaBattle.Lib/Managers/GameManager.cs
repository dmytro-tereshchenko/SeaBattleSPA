using System;
using System.Collections.Generic;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Responses;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for game flow of "Sea battle"
    /// </summary>
    public class GameManager : IGameManager
    {
        /// <summary>
        /// Manager which response for creating and initializing start entities.
        /// </summary>
        /// <value><see cref="IInitializeManager"/></value>
        private readonly IInitializeManager _initializeManager;

        /// <summary>
        /// Manager which response for creating and change ships.
        /// </summary>
        /// <value><see cref="IShipManager"/></value>
        private readonly IShipManager _shipManager;

        /// <summary>
        /// Manager which response for the actions and changes of ships on the field during the game.
        /// </summary>
        /// <value><see cref="IActionManager"/></value>
        private readonly IActionManager _actionManager;

        /// <summary>
        /// Current game
        /// </summary>
        /// <value><see cref="IGame"/></value>
        private IGame _game;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeManager"/> class
        /// </summary>
        /// <param name="factory">Abstract factory for initializing</param>
        public GameManager(IAbstractGameFactory factory)
        {
            _initializeManager = factory.GetInitializeManager();
            _shipManager = factory.GetShipManager();
            _actionManager = factory.GetActionManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeManager"/> class
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="factory">Abstract factory for initializing</param>
        public GameManager(IAbstractGameFactory factory, IGame game) : this(factory) => _game = game;

        /// <summary>
        /// Create <see cref="IGame"/>
        /// </summary>
        /// <param name="numberOfPlayers">Number of players in <see cref="IGame"/></param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode CreateGame(byte numberOfPlayers)
        {
            if (_game != null)
            {
                return StateCode.ErrorInitialization;
            }

            try
            {
                _game = _initializeManager.CreateGame(numberOfPlayers);
            }
            catch (ArgumentOutOfRangeException)
            {
                return StateCode.ExceededMaxNumberOfPlayers;
            }

            return StateCode.Success;
        }

        /// <summary>
        /// Create <see cref="IGameField"/>
        /// </summary>
        /// <param name="sizeX">SizeX of <see cref="IGameField"/></param>
        /// <param name="sizeY">SizeY of <see cref="IGameField"/></param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode CreateGameField(ushort sizeX, ushort sizeY)
        {
            if (_game == null || _game.Field != null)
            {
                return StateCode.ErrorInitialization;
            }

            IResponseGameField response = _initializeManager.CreateGameField(sizeX, sizeY);
            
            if (response.State == StateCode.Success)
            {
                _game.Field = response.Value;
            }

            return response.State;
        }

        /// <summary>
        /// Add player to the game
        /// </summary>
        /// <param name="playerName">Player's name</param>
        /// <returns><see cref="IResponseGamePlayer"/></returns>
        public IResponseGamePlayer AddGamePlayer(string playerName) =>
            _initializeManager.AddPlayerToGame(_game, playerName);

        /// <summary>
        /// Get start field by player and game. In case absence of starting fields, create them.
        /// </summary>
        /// <param name="player">The player who request start field</param>
        /// <returns><see cref="IResponseStartField"/></returns>
        public IResponseStartField GetStartField(IGamePlayer player) => _initializeManager.GetStartField(_game, player);

        /// <summary>
        /// Get actual game field
        /// </summary>
        /// <param name="player">The player who request game field</param>
        /// <returns><see cref="IResponseGameField"/></returns>
        public IResponseGameField GetGameField(IGamePlayer player) => _actionManager.GetGameField(player, _game);

        /// <summary>
        /// Getting a collection of ships, which players can buy by points.
        /// </summary>
        /// <returns>Collection of <see cref="ICommonShip"/>, which players can buy by points</returns>
        public ICollection<ICommonShip> GetShips() => _shipManager.GetShips();

        /// <summary>
        /// Getting a collection of weapons, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IWeapon"/>, which players can equip on the ship</returns>
        public ICollection<IWeapon> GetWeapons() => _shipManager.GetWeapons();

        /// <summary>
        /// Getting a collection of repairs, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IRepair"/>, which players can equip on the ship</returns>
        public ICollection<IRepair> GetRepairs() => _shipManager.GetRepairs();

        /// <summary>
        /// Buy ship and save in <see cref="IStartField"/>
        /// </summary>
        /// <param name="player">The player who request buying ship</param>
        /// <param name="ship">The base ship that is buying</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode BuyShip(IGamePlayer player, ICommonShip ship)
        {
            IResponseStartField response = _initializeManager.GetStartField(_game, player);

            return (response.State != StateCode.Success)
                ? response.State
                : _shipManager.BuyShip(_game.Players, _shipManager.GetNewShip(player, ship), response.Value);
        }

        /// <summary>
        /// Sell ship and save in <see cref="IStartField"/>
        /// </summary>
        /// <param name="player">The player who request selling ship</param>
        /// <param name="ship">The ship that is selling</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode SellShip(IGamePlayer player, IGameShip ship)
        {
            IResponseStartField response = _initializeManager.GetStartField(_game, player);

            return (response.State != StateCode.Success)
                ? response.State
                : _shipManager.SellShip(_game.Players, ship, response.Value);
        }

        /// <summary>
        /// Add weapon to game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Game ship which adds a weapon.</param>
        /// <param name="weapon">A weapon (<see cref="IWeapon"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode AddWeapon(IGamePlayer player, IGameShip ship, IWeapon weapon) =>
            _shipManager.AddWeapon(player, ship, weapon);

        /// <summary>
        /// Add repair to game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Game ship which adds a repair.</param>
        /// <param name="repair">A repair (<see cref="IRepair"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode AddRepair(IGamePlayer player, IGameShip ship, IRepair repair) =>
            _shipManager.AddRepair(player, ship, repair);

        /// <summary>
        /// Remove weapon from game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Game ship which removes a weapon.</param>
        /// <param name="weapon">Weapon (<see cref="IWeapon"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode RemoveWeapon(IGamePlayer player, IGameShip ship, IWeapon weapon) =>
            _shipManager.RemoveWeapon(player, ship, weapon);

        /// <summary>
        /// Remove repair from game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Game ship which removes a repair.</param>
        /// <param name="repair">Repair (<see cref="IRepair"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode RemoveRepair(IGamePlayer player, IGameShip ship, IRepair repair) =>
            _shipManager.RemoveRepair(player, ship, repair);
    }
}
