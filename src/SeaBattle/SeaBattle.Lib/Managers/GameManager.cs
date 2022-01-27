using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Get sorted collection of the ship by distance to the center of the field.
        /// </summary>
        /// <param name="player">Current player for filtering <see cref="IGameShip"/>, if null - get all ships without filter.</param>
        /// <returns><see cref="IDictionary{TKey,TValue}"/> whose generic key argument is <see cref="IGameShip"/>, generic type argument
        /// is <see cref="ICollection{T}"/> whose generic type argument is (<see cref="ushort"/>, <see cref="ushort"/>) coordinates (X,Y)</returns>
        IDictionary<IGameShip, ICollection<(ushort, ushort)>> GetSortedShipsFromField(IGamePlayer player = null) =>
            _actionManager.GetFieldWithShips(_game.Field, player);

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

        /// <summary>
        /// Place ship on game field
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Target ship</param>
        /// <param name="posX">X coordinate of the ship's stern</param>
        /// <param name="posY">Y coordinate of the ship's stern</param>
        /// <param name="direction">The direction of placement ship</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode PutShipOnField(IGamePlayer player, IGameShip ship, ushort posX, ushort posY,
            DirectionOfShipPosition direction)
        {
            IResponseStartField response = _initializeManager.GetStartField(_game, player);

            if (response.State != StateCode.Success)
            {
                return response.State;
            }

            return (response.Value.Ships.Contains(ship))
                ? _actionManager.TransferShipToGameField(player, posX, posY, direction, response.Value, ship)
                : _actionManager.MoveShip(player, ship, posX, posY, direction, _game.Field);
        }

        /// <summary>
        /// Remove <see cref="IGameShip"/> from <see cref="IGameField"/> to collection in <see cref="IStartField.Ships"/>
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="posX">X coordinate of removed ship</param>
        /// <param name="posY">Y coordinate of removed ship</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode RemoveShipFromField(IGamePlayer player, ushort posX, ushort posY)
        {
            IResponseStartField response = _initializeManager.GetStartField(_game, player);

            return (response.State != StateCode.Success)
                ? response.State
                : _actionManager.TransferShipFromGameField(player, posX, posY, response.Value);
        }

        /// <summary>
        /// Set state ready for player (finished buying ships and placing them on the game field)
        /// </summary>
        /// <param name="player">Current player</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode ReadyPlayer(IGamePlayer player)
        {
            if (!_game.Players.Contains(player))
            {
                return StateCode.InvalidPlayer;
            }

            player.State = PlayerState.Ready;

            if (_game.Players.All(p => p.State == PlayerState.Ready))
            {
                foreach (var gamePlayer in _game.Players)
                {
                    gamePlayer.State = PlayerState.Process;
                }

                _game.State = GameState.Process;

                //first player who get startField - first moves
                _game.CurrentGamePlayerMove = _game.StartFields.First().GamePlayer;
            }

            return StateCode.Success;
        }

        /// <summary>
        /// Get <see cref="ICollection{T}"/> of <see cref="IGameShip"/> on distance of action.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="action">Type of possible actions (<see cref="ActionType.Attack"/>, <see cref="ActionType.Repair"/>)</param>
        /// <returns><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"/>, otherwise default</returns>
        public ICollection<IGameShip> GetVisibletargetsforShip(IGamePlayer player, IGameShip ship, ActionType action)
        {
            ICollection<IGameShip> result;

            try
            {
                result = _actionManager.GetVisibleTargetsForShip(player, ship, _game.Field, action);
            }
            catch (Exception ex)
            {
                result = new List<IGameShip>();
            }

            return result;
        }

        /// <summary>
        /// Attack cell by ship
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="posX">X coordinate of target cell</param>
        /// <param name="posY">Y coordinate of target cell</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode AttackShip(IGamePlayer player, IGameShip ship, ushort posX, ushort posY) =>
            _actionManager.AttackShip(player, ship, posX, posY, _game.Field);

        /// <summary>
        /// Repair ship by cell
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <param name="posX">X coordinate of target cell</param>
        /// <param name="posY">Y coordinate of target cell</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode RepairShip(IGamePlayer player, IGameShip ship, ushort posX, ushort posY) =>
            _actionManager.RepairShip(player, ship, posX, posY, _game.Field);

        /// <summary>
        /// Repair all friendly ship on distance
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="ship">Current ship</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode RepairAllShip(IGamePlayer player, IGameShip ship) =>
            _actionManager.RepairAllShip(player, ship, _game.Field);

        protected void NextMove()
        {
            ICollection<IGamePlayer> players = _game.Players;

            for (int i = 0; i < players.Count; i++)
            {
                if (players.ElementAt(i) == _game.CurrentGamePlayerMove)
                {
                    _game.CurrentGamePlayerMove =
                        i + 1 < players.Count ? players.ElementAt(i + 1) : players.ElementAt(0);
                    return;
                }
            }
        }
    }
}
