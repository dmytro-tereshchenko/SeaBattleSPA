using SeaBattle.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Repositories;
using SeaBattle.Lib.Responses;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for creating and initializing start entities, implements <see cref="IInitializeManager"/>.
    /// </summary>
    public class InitializeManager : IInitializeManager
    {
        private readonly IShipStorageUtility _storageUtility;

        private readonly IGameConfig _gameConfig;

        private readonly GenericRepository<Game> _gameRepository;

        private readonly GenericRepository<GameField> _gameFieldRepository;

        private readonly GenericRepository<GamePlayer> _gamePlayerRepository;

        private readonly GenericRepository<StartField> _startFieldRepository;

        private readonly GenericRepository<StartFieldCell> _startFieldCellRepository;

        public InitializeManager(IGameConfig gameConfig, IShipStorageUtility storageUtility,
            GenericRepository<Game> gameRepository, GenericRepository<GameField> gameFieldRepository,
            GenericRepository<GamePlayer> gamePlayerRepository, GenericRepository<StartField> startFieldRepository,
            GenericRepository<StartFieldCell> startFieldCellRepository)
        {
            _gameConfig = gameConfig;
            _storageUtility = storageUtility;
            _gameRepository = gameRepository;
            _gameFieldRepository = gameFieldRepository;
            _gamePlayerRepository = gamePlayerRepository;
            _startFieldRepository = startFieldRepository;
            _startFieldCellRepository = startFieldCellRepository;
        }

        public async Task<IResponseGameField> CreateGameField(int gameId, ushort sizeX, ushort sizeY)
        {
            if (sizeX < _gameConfig.MinFieldSizeX || sizeX > _gameConfig.MaxFieldSizeX ||
                sizeY < _gameConfig.MinFieldSizeY || sizeY > _gameConfig.MaxFieldSizeY)
            {
                return new ResponseGameField(null, StateCode.InvalidFieldSize);
            }

            Game game = await _gameRepository.FindByIdAsync(gameId);

            if (game == null)
            {
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(CreateGameField)}");
                ex.Data.Add("gameId", gameId);

                throw ex;
            }

            GameField field = new GameField(sizeX, sizeY) {GameId = gameId};

            field = await _gameFieldRepository.CreateAsync(field);

            return new ResponseGameField(field, StateCode.Success);
        }

        public int CalculateStartPoints(ICollection<StartFieldCell> startFieldCells, ushort sizeX, ushort sizeY)
        {
            //Find maximum count ships with minimum size which we can place on the start field.
            int countShips = 0;
            bool[,] ships = new bool[sizeX, sizeY];

            //Iterate the entire field
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    //Check if the cell is for the placement ship and around we don't have other ships.
                    if (startFieldCells.FirstOrDefault(c => c.X == i + 1 && c.Y == j + 1) != null &&
                        CheckFreeAreaAroundShip(ships, i, j))
                    {
                        //place and count ship
                        ships[i, j] = true;
                        countShips++;
                    }
                }
            }

            //Return the total cost of ships.
            return countShips * _storageUtility.CalculatePointCost(1);
        }

        public LimitSize GetLimitSizeField() =>
            new LimitSize(_gameConfig.MaxFieldSizeX, _gameConfig.MaxFieldSizeY, _gameConfig.MinFieldSizeX,
                _gameConfig.MinFieldSizeY);

        public byte GetMaxNumberOfPlayers() => _gameConfig.MaxNumberOfPlayers;

        public async Task<IResponseGame> AddPlayerToGame(int gameId, string playerName)
        {
            var queryGame =
                await _gameRepository.GetWithIncludeAsync(g => g.Id == gameId, g => g.GamePlayers);
            Game game = queryGame.FirstOrDefault();

            if (game is null)
            {
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(AddPlayerToGame)}");
                ex.Data.Add("gameId", gameId);

                throw ex;
            }

            if (game.CurrentCountPlayers == game.MaxNumberOfPlayers)
            {
                return new ResponseGame(null, StateCode.ExceededMaxNumberOfPlayers);
            }

            if (game.GamePlayers.Count == 0)
            {
                game.GameState = GameState.SearchPlayers;
            }

            var queryPlayer = await _gamePlayerRepository.GetAsync(p => p.Name == playerName);
            GamePlayer gamePlayer = queryPlayer.FirstOrDefault();

            if (gamePlayer is null)
            {
                gamePlayer = await _gamePlayerRepository.CreateAsync(new GamePlayer(playerName));
            }

            game.GamePlayers.Add(gamePlayer);

            if (game.GamePlayers.Count == game.MaxNumberOfPlayers)
            {
                game.GameState = GameState.Init;
            }

            game = await _gameRepository.UpdateAsync(game);

            return new ResponseGame(game, StateCode.Success);
        }

        public async Task<IResponseStartField> GetStartField(int gameId, string gamePlayerName)
        {
            var queryGame =
                await _gameRepository.GetWithIncludeAsync(g => g.Id == gameId,
                g => g.StartFields,
                g => g.GamePlayers,
                g => g.GameField);
            Game game = queryGame.FirstOrDefault();

            var queryPlayer = await _gamePlayerRepository.GetWithIncludeAsync(g => g.Name == gamePlayerName);
            GamePlayer gamePlayer = queryPlayer.FirstOrDefault();

            if (game is null || gamePlayer is null)
            {
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(GetStartField)}");
                ex.Data.Add("gameId", gameId);
                ex.Data.Add("gamePlayerName", gamePlayerName);

                throw ex;
            }

            //Variable for result
            StartField startField = null;

            //in the case haven't created startFields - create them
            if (game.StartFields is null || game.StartFields.Count == 0)
            {
                game.StartFields = new List<StartField>(game.MaxNumberOfPlayers);
                ICollection<ICollection<StartFieldCell>> fieldsOfLabels;

                fieldsOfLabels =
                    GenerateStartFields(game.GameField.SizeX, game.GameField.SizeY, game.MaxNumberOfPlayers);

                //calculate start points for minimum size fields
                int startPoints = fieldsOfLabels
                    .Select(f => CalculateStartPoints(f, game.GameField.SizeX, game.GameField.SizeY)).Min();

                foreach (var labelField in fieldsOfLabels)
                {
                    game.StartFields.Add(
                        new StartField(game.GameField, labelField, null, startPoints, new List<GameShip>(), game.Id)
                        {
                            StartFieldCells = labelField
                        });
                }

                //get first of start fields for the current player
                startField = game.StartFields.FirstOrDefault();
            }
            //in the case bd have start field for current player and game return it
            else if ((startField = game.StartFields.FirstOrDefault(f => f.GamePlayerId == gamePlayer.Id)) is not null)
            {
                var queryStartField = await _startFieldRepository.GetWithIncludeAsync(f => f.Id == startField.Id,
                f => f.StartFieldCells,
                f => f.GameShips);

                return new ResponseStartField(queryStartField.FirstOrDefault(), StateCode.Success);
            }
            //otherwise get first of free start fields.
            else
            {
                startField = game.StartFields.FirstOrDefault(f => f.GamePlayer is null);
            }

            //add current player to start field
            if (startField is not null)
            {
                startField.GamePlayer = gamePlayer;

                //change status player
                startField.GamePlayer.PlayerState = PlayerState.InitializeField;

                await _gameRepository.UpdateAsync(game);

                return new ResponseStartField(startField, StateCode.Success);
            }

            //not free position for player in the current game
            return new ResponseStartField(null, StateCode.InvalidPlayer);
        }

        public async Task<IGame> CreateGame(byte numberOfPlayers)
        {
            if (numberOfPlayers < 1 || numberOfPlayers > _gameConfig.MaxNumberOfPlayers)
            {
                Exception ex = new ArgumentOutOfRangeException(
                    $"{nameof(numberOfPlayers)} is out of range [0;{_gameConfig.MaxNumberOfPlayers}] in progress {nameof(CreateGame)}");

                throw ex;
            }

            Game game = new Game(numberOfPlayers);

            game = await _gameRepository.CreateAsync(game);

            return game;
        }

        public async Task<StateCode> ReadyPlayer(int gameId, string gamePlayerName)
        {
            var queryGame =
                await _gameRepository.GetWithIncludeAsync(g => g.Id == gameId, g => g.GamePlayers);
            Game game = queryGame.FirstOrDefault();

            var queryPlayer = await _gamePlayerRepository.GetWithIncludeAsync(g => g.Name == gamePlayerName);
            GamePlayer player = queryPlayer.FirstOrDefault();

            if (game is null || player is null)
            {
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(ReadyPlayer)}");
                ex.Data.Add("gameId", gameId);
                ex.Data.Add("gamePlayerName", gamePlayerName);

                throw ex;
            }

            if (game.GameState == GameState.Finished)
            {
                return StateCode.GameFinished;
            }

            if (!game.GamePlayers.Contains(player))
            {
                return StateCode.InvalidPlayer;
            }

            player.PlayerState = PlayerState.Ready;

            if (game.GamePlayers.All(p => p.PlayerState == PlayerState.Ready))
            {
                foreach (var gamePlayer in game.GamePlayers)
                {
                    gamePlayer.PlayerState = PlayerState.Process;
                }

                game.GameState = GameState.Process;

                //first player who get startField - first moves
                game.CurrentGamePlayerMoveId = game.StartFields.First().GamePlayerId;
            }

            await _gameRepository.UpdateAsync(g => g.Id == game.Id, game.GamePlayers,
                        _gamePlayerRepository.GetAll(), "GamePlayers");

            return StateCode.Success;
        }

        /// <summary>
        /// Method for checking free area around ship with size equals 1
        /// </summary>
        /// <param name="field">Field with ships - array with type bool, where true - placed ship, false - empty.</param>
        /// <param name="x">Coordinate X where placed ship which we relatively check free area.</param>
        /// <param name="y">Coordinate Y where placed ship which we relatively check free area.</param>
        /// <returns>false - there is ship around target cell, otherwise true - around area is free.</returns>
        protected bool CheckFreeAreaAroundShip(bool[,] field, int x, int y)
        {
            int sizeX = field.GetLength(0);
            int sizeY = field.GetLength(1);

            if (x < 0 || x >= sizeX || y < 0 || y > sizeY)
            {
                Exception ex =
                    new Exception($"Invalid coordinates of cell in progress {nameof(CheckFreeAreaAroundShip)}");
                ex.Data.Add("field", field);
                ex.Data.Add("x", x);
                ex.Data.Add("y", y);

                throw ex;
            }

            int offsetX = -1;
            int offsetY = 0;
            int offsetXY;
            int offsetYX;

            for (int i = 0; i < 4; i++)
            {
                //Check free cells on diagonal from the cell with coordinates x,y
                offsetXY = Convert.ToInt32(Math.Pow(-1, i / 2));
                offsetYX = Convert.ToInt32(Math.Pow(-1, (i + 1) / 2));
                if (x + offsetXY >= 0 && x + offsetXY < sizeX && y + offsetYX >= 0 && y + offsetYX < sizeY &&
                    field[x + offsetXY, y + offsetYX] == true)
                {
                    return false;
                }

                //Check free cells on horizontal and vertical from the cell with coordinates x,y
                if (x + offsetX >= 0 && x + offsetX < sizeX && y + offsetY >= 0 && y + offsetY < sizeY &&
                    field[x + offsetX, y + offsetY] == true)
                {
                    return false;
                }

                //Change offset for check horizontal and vertical sides.
                offsetX += Convert.ToInt32(Math.Pow(-1, i / 2));
                offsetY += Convert.ToInt32(Math.Pow(-1, (i + 1) / 2));
            }

            //If we don't find a ship that area is free.
            return true;
        }

        /// <summary>
        /// Method for generating a collection of fields with labels for possible placing ships on start field for teams.
        /// </summary>
        /// <param name="sizeX">Size X of field of the game</param>
        /// <param name="sizeY">Size Y of field of the game</param>
        /// <param name="numberOfPlayers">Amount of players</param>
        /// <returns>Collection of fields where collection of <see cref="StartFieldCell"/>with coordinates possible to place boat</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="numberOfPlayers"/> out of range</exception>
        protected ICollection<ICollection<StartFieldCell>> GenerateStartFields(ushort sizeX, ushort sizeY,
            byte numberOfPlayers)
        {
            if (numberOfPlayers > _gameConfig.MaxNumberOfPlayers || numberOfPlayers <= 0)
            {
                Exception ex =
                    new Exception($"Invalid value of the number of players in progress {nameof(GenerateStartFields)}");
                ex.Data.Add("numberOfPlayers", numberOfPlayers);

                throw ex;
            }

            //List for result of method
            ICollection<ICollection<StartFieldCell>> fields = new List<ICollection<StartFieldCell>>(numberOfPlayers);

            //Amount of rows and columns for the position of teams.
            ushort colTeam = (ushort) Math.Ceiling(Math.Sqrt((double) numberOfPlayers));
            ushort rowTeam = (ushort) Math.Ceiling((double) numberOfPlayers / colTeam);

            //Coordinates of begin and end every row and column of quadrant for every team. 
            (ushort begin, ushort end)[] borderQuadrantsX = new (ushort, ushort)[rowTeam];
            (ushort begin, ushort end)[] borderQuadrantsY = new (ushort, ushort)[colTeam];

            //Calculation begins and ends of quadrants.
            for (ushort i = 0; i < rowTeam; i++)
            {
                borderQuadrantsX[i].begin = (ushort) (i == 0 ? 0 : i * sizeX / rowTeam + 1);
                borderQuadrantsX[i].end = (ushort) (i + 1 == rowTeam ? sizeX - 1 : (i + 1) * sizeX / rowTeam - 1);
            }

            for (int i = 0; i < colTeam; i++)
            {
                borderQuadrantsY[i].begin = (ushort) (i == 0 ? 0 : i * sizeY / colTeam + 1);
                borderQuadrantsY[i].end = (ushort) (i + 1 == colTeam ? sizeY - 1 : (i + 1) * sizeY / colTeam - 1);
            }

            //Coordinates of quadrant for the team.
            int quadrantX, quadrantY;

            //Add fields until the amount of fields = number of teams.
            while (fields.Count < numberOfPlayers)
            {
                //Calculation of coordinates of quadrant for the team.
                quadrantX = fields.Count / colTeam;
                quadrantY = fields.Count - quadrantX * colTeam;

                ICollection<StartFieldCell> startField = new List<StartFieldCell>();

                //Set true for chosen part of the field (quadrant).
                for (ushort i = borderQuadrantsX[quadrantX].begin; i <= borderQuadrantsX[quadrantX].end; i++)
                {
                    for (ushort j = borderQuadrantsY[quadrantY].begin; j <= borderQuadrantsY[quadrantY].end; j++)
                    {
                        startField.Add(new StartFieldCell() {X = (ushort) (i + 1), Y = (ushort) (j + 1)});
                    }
                }

                fields.Add(startField);
            }

            return fields;
        }
    }
}
