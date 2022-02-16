using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

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

        public string CurrentGamePlayerMove
        {
            get => _game.CurrentGamePlayerMove.ToString();
        }

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

        #region Data access

        /*public StateCode CreateGame(byte numberOfPlayers)
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

        public IResponseGamePlayer AddGamePlayer(string playerName) =>
            _initializeManager.AddPlayerToGame(_game, playerName);

        public IResponseStartField GetStartField(IGamePlayer player) => _initializeManager.GetStartField(_game, player);

        public IResponseGameField GetGameField(IGamePlayer player) => _actionManager.GetGameField(player, _game);

        public ICollection<(IShip, int)> GetShips() => _shipManager.GetShips();

        public ICollection<IWeapon> GetWeapons() => _shipManager.GetWeapons();

        public ICollection<IRepair> GetRepairs() => _shipManager.GetRepairs();

        public IDictionary<IGameShip, ICollection<(ushort, ushort)>> GetSortedShipsFromField(IGamePlayer player = null) =>
            _actionManager.GetFieldWithShips(_game.Field, player);

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

        public IGamePlayer GetResultGame() => _game.Winner;

        public LimitSize GetLimitSizeField() => _initializeManager.GetLimitSizeField();

        public byte GetMaxNumberOfPlayers() => _game?.MaxNumberOfPlayers ?? _initializeManager.GetMaxNumberOfPlayers();

        #endregion

        #region Update ship

        public StateCode BuyShip(IGamePlayer player, IShip ship)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            IResponseStartField response = _initializeManager.GetStartField(_game, player);

            return (response.State != StateCode.Success)
                ? response.State
                : _shipManager.BuyShip(_game.GamePlayers, _shipManager.GetNewShip(player, ship), response.Value);
        }

        public StateCode SellShip(IGamePlayer player, IGameShip ship)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            IResponseStartField response = _initializeManager.GetStartField(_game, player);

            return (response.State != StateCode.Success)
                ? response.State
                : _shipManager.SellShip(_game.GamePlayers, ship, response.Value);
        }

        public StateCode AddWeapon(IGamePlayer player, IGameShip ship, IWeapon weapon)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            return _shipManager.AddWeapon(player, ship, weapon);
        }

        public StateCode AddRepair(IGamePlayer player, IGameShip ship, IRepair repair)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            return _shipManager.AddRepair(player, ship, repair);
        }

        public StateCode RemoveWeapon(IGamePlayer player, IGameShip ship, IWeapon weapon)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            return _shipManager.RemoveWeapon(player, ship, weapon);
        }

        public StateCode RemoveRepair(IGamePlayer player, IGameShip ship, IRepair repair)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            return _shipManager.RemoveRepair(player, ship, repair);
        }

        #endregion

        #region Actions

        public StateCode PutShipOnField(IGamePlayer player, IGameShip ship, ushort posX, ushort posY,
            DirectionOfShipPosition direction)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            IResponseStartField response = _initializeManager.GetStartField(_game, player);

            if (response.State != StateCode.Success)
            {
                return response.State;
            }

            return (response.Value.Ships.Contains(ship))
                ? _actionManager.TransferShipToGameField(player, posX, posY, direction, response.Value, ship)
                : _actionManager.MoveShip(player, ship, posX, posY, direction, _game.Field);
        }

        public StateCode RemoveShipFromField(IGamePlayer player, ushort posX, ushort posY)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            IResponseStartField response = _initializeManager.GetStartField(_game, player);

            return (response.State != StateCode.Success)
                ? response.State
                : _actionManager.TransferShipFromGameField(player, posX, posY, response.Value);
        }

        public StateCode ReadyPlayer(IGamePlayer player)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            if (!_game.GamePlayers.Contains(player))
            {
                return StateCode.InvalidPlayer;
            }

            player.State = PlayerState.Ready;

            if (_game.GamePlayers.All(p => p.State == PlayerState.Ready))
            {
                foreach (var gamePlayer in _game.GamePlayers)
                {
                    gamePlayer.State = PlayerState.Process;
                }

                _game.State = GameState.Process;

                //first player who get startField - first moves
                _game.CurrentGamePlayerMove = _game.StartFields.First().GamePlayer;
            }

            return StateCode.Success;
        }

        public StateCode AttackShip(IGamePlayer player, IGameShip ship, ushort posX, ushort posY)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            StateCode state = _actionManager.AttackShip(player, ship, posX, posY, _game.Field);

            return CheckEndGame() ? StateCode.GameFinished : state;
        }

        public StateCode RepairShip(IGamePlayer player, IGameShip ship, ushort posX, ushort posY)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            return _actionManager.RepairShip(player, ship, posX, posY, _game.Field);
        }

        public StateCode RepairAllShip(IGamePlayer player, IGameShip ship)
        {
            if (_game.State == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            return _actionManager.RepairAllShip(player, ship, _game.Field);
        }

        #endregion

        #region Services

        public StateCode NextMove()
        {
            ICollection<IGamePlayer> players = _game.GamePlayers;

            for (int i = 0; i < players.Count; i++)
            {
                if (players.ElementAt(i) == _game.CurrentGamePlayerMove)
                {
                    _game.CurrentGamePlayerMove =
                        i + 1 < players.Count ? players.ElementAt(i + 1) : players.ElementAt(0);

                    return StateCode.Success;
                }
            }

            return StateCode.InvalidPlayer;
        }

        /// <summary>
        /// Check end game after action and change state <see cref="IGame"/>
        /// </summary>
        /// <returns>true if game finished, otherwise false</returns>
        protected bool CheckEndGame()
        {
            IGamePlayer player = null;
            for (ushort i = 1; i <= _game.Field.SizeX; i++)
            {
                for (ushort j = 1; j < _game.Field.SizeY; j++)
                {
                    if (_game.Field[i, j] != null)
                    {
                        if (player != null && _game.Field[i, j].GamePlayer != player)
                        {
                            return false;
                        }
                        else
                        {
                            player = _game.Field[i, j].GamePlayer;
                        }
                    }
                }
            }

            _game.State = GameState.Finished;
            _game.Winner = player;

            return true;
        }*/

#endregion

    }
}
