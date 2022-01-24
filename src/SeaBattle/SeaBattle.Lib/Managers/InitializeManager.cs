using SeaBattle.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for creating and initializing start entities, implements <see cref="IInitializeManager"/>.
    /// </summary>
    public class InitializeManager : IInitializeManager
    {
        private const int PriceCoefficient = 1000;

        private IUnitOfWork _repository;

        private ushort _minSizeX;

        private ushort _maxSizeX;

        private ushort _minSizeY;

        private ushort _maxSizeY;

        private byte _maxNumberOfTeams;

        public InitializeManager(IUnitOfWork repository, ushort minSizeX, ushort maxSizeX, ushort minSizeY, ushort maxSizeY, byte maxNumberOfTeams)
        {
            _repository = repository;
            _minSizeX = minSizeX;
            _maxSizeX = maxSizeX;
            _minSizeY = minSizeY;
            _maxSizeY = maxSizeY;
            _maxNumberOfTeams = maxNumberOfTeams;
        }

        /// <summary>
        /// Creating and getting game field.
        /// </summary>
        /// <param name="sizeX">Size X of created game field.</param>
        /// <param name="sizeY">Size Y of created game field.</param>
        /// <returns><see cref="IResponseGameField"/> where Value is <see cref="IGameField"/>, State is <see cref="StateCode"/></returns>
        public async Task<IResponseGameField> CreateGameFieldAsync(ushort sizeX, ushort sizeY)
        {
            if (sizeX < _minSizeX || sizeX > _maxSizeX || sizeY < _minSizeY || sizeY > _maxSizeY)
            {
                return new ResponseGameField(null, StateCode.InvalidFieldSize);
            }

            GameField field = new GameField(sizeX, sizeY);
            _repository.GameFields.Create(field);

            await _repository.SaveAsync();

            return new ResponseGameField(field, StateCode.Success);
        }

        /// <summary>
        /// Method for calculation points which need to purchase ships.
        /// </summary>
        /// <param name="field">Field with placement ships on start game - array with type bool, where true - can placed ship, false - wrong cell.</param>
        /// <returns>Amount of points</returns>
        public int CalculateStartPoints(bool[,] field)
        {
            //Find maximum count ships with minimum size which we can place on the start field.
            int sizeX = field.GetLength(0);
            int sizeY = field.GetLength(1);

            int countShips = 0;
            bool[,] ships = new bool[sizeX, sizeY];

            //Iterate the entire field
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    //Check if the cell is for the placement ship and around we don't have other ships.
                    if (field[i, j] == true && CheckFreeAreaAroundShip(ships, i, j))
                    {
                        //place and count ship
                        ships[i, j] = true;
                        countShips++;
                    }
                }
            }

            //Return the total cost of ships.
            return countShips * GetShipCost(1);
        }

        /// <summary>
        /// Method for checking free area around ship with size equals 1
        /// </summary>
        /// <param name="field">Field with ships - array with type bool, where true - placed ship, false - empty.</param>
        /// <param name="x">Coordinate X where placed ship which we relatively check free area.</param>
        /// <param name="y">Coordinate Y where placed ship which we relatively check free area.</param>
        /// <returns>true - there is ship around target cell, otherwise false - around area is free.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="x"/>, <paramref name="y"/> is out of range game field.</exception>
        private bool CheckFreeAreaAroundShip(bool[,] field, int x, int y)
        {
            int sizeX = field.GetLength(0);
            int sizeY = field.GetLength(1);

            if (x < 0 || x >= sizeX || y < 0 || y > sizeY)
            {
                throw new ArgumentOutOfRangeException();
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
        /// Buy ship and add to <see cref="IStartField"/>
        /// </summary>
        /// <param name="playerId">Id of team</param>
        /// <param name="gameShipId">Id of game ship</param>
        /// <param name="startFieldId">Id of start field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public async Task<StateCode> BuyShipAsync(uint playerId, uint gameShipId, uint startFieldId)
        {
            IStartField field = _repository.StartFields.Get(startFieldId);
            IPlayer team = _repository.Players.Get(playerId);
            IGameShip ship = _repository.GameShips.Get(gameShipId);

            if (field == null || team == null || ship == null)
            {
                return StateCode.InvalidId;
            }

            if (field.PlayerId != playerId)
            {
                return StateCode.InvalidTeam;
            }

            if (ship.Points > field.Points)
            {
                return StateCode.PointsShortage;
            }

            field.Ships.Add(ship);
            field.Points -= ship.Points;

            _repository.StartFields.Update(field);
            await _repository.SaveAsync();

            return StateCode.Success;
        }

        /// <summary>
        /// Sell ship and remove from <see cref="IStartField"/>
        /// </summary>
        /// <param name="playerId">Id of team</param>
        /// <param name="gameShipId">Id of game ship</param>
        /// <param name="startFieldId">Id of start field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public async Task<StateCode> SellShipAsync(uint playerId, uint gameShipId, uint startFieldId)
        {
            IStartField field = _repository.StartFields.Get(startFieldId);
            IPlayer team = _repository.Players.Get(playerId);
            IGameShip ship = _repository.GameShips.Get(gameShipId);

            if (field == null || team == null || ship == null)
            {
                return StateCode.InvalidId;
            }

            if (field.PlayerId != playerId)
            {
                return StateCode.InvalidTeam;
            }

            field.Ships.Remove(ship);
            field.Points += ship.Points;

            _repository.Ships.Delete(ship.Ship.Id);
            _repository.GameShips.Delete(ship.Id);
            _repository.StartFields.Update(field);

            await _repository.SaveAsync();
            return StateCode.Success;
        }

        /// <summary>
        /// Method for generating a collection of fields with labels for possible placing ships on start field for teams.
        /// </summary>
        /// <param name="gameFieldId">id of gaming field.</param>
        /// <param name="numberOfTeams">Amount of teams</param>
        /// <returns>Collection of fields with ships - arrays with type bool, where true - placed boat, false - empty</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="numberOfTeams"/> out of range</exception>
        /// <exception cref="ArgumentNullException">There is no gaming field for the given <paramref name="gameFieldId"/>.</exception>
        public ICollection<bool[,]> GenerateStartFields(uint gameFieldId, byte numberOfTeams)
        {
            if (numberOfTeams > _maxNumberOfTeams || numberOfTeams <= 0)
            {
                throw new ArgumentOutOfRangeException("Wrong value of the number of teams");
            }

            IGameField field = _repository.GameFields.Get(gameFieldId);
            if (field == null)
            {
                throw new ArgumentNullException("There is no playing field for the given id.");
            }

            //List for result of method
            ICollection<bool[,]> fields = new List<bool[,]>(numberOfTeams);

            //Amount of rows and columns for the position of teams.
            int colTeam = (int)Math.Ceiling(Math.Sqrt((double)numberOfTeams));
            int rowTeam = (int)Math.Ceiling((double)numberOfTeams / colTeam);

            //Coordinates of begin and end every row and column of quadrant for every team. 
            (int begin, int end)[] borderQuadrantsX = new (int, int)[rowTeam];
            (int begin, int end)[] borderQuadrantsY = new (int, int)[colTeam];

            //Calculation begins and ends of quadrants.
            for (int i = 0; i < rowTeam; i++)
            {
                borderQuadrantsX[i].begin = i == 0 ? 0 : i * field.SizeX / rowTeam + 1;
                borderQuadrantsX[i].end = i + 1 == rowTeam ? field.SizeX - 1 : (i + 1) * field.SizeX / rowTeam - 1;
            }

            for (int i = 0; i < colTeam; i++)
            {
                borderQuadrantsY[i].begin = i == 0 ? 0 : i * field.SizeY / colTeam + 1;
                borderQuadrantsY[i].end = i + 1 == colTeam ? field.SizeY - 1 : (i + 1) * field.SizeY / colTeam - 1;
            }

            //Coordinates of quadrant for the team.
            int quadrantX, quadrantY;

            //Add fields until the amount of fields = number of teams.
            while (fields.Count < numberOfTeams)
            {
                //Calculation of coordinates of quadrant for the team.
                quadrantX = fields.Count / colTeam;
                quadrantY = fields.Count - quadrantX * colTeam;

                bool[,] startField = new bool[field.SizeX, field.SizeY];

                //Set true for chosen part of the field (quadrant).
                for (int i = borderQuadrantsX[quadrantX].begin; i <= borderQuadrantsX[quadrantX].end; i++)
                {
                    for (int j = borderQuadrantsY[quadrantY].begin; j <= borderQuadrantsY[quadrantY].end; j++)
                    {
                        startField[i, j] = true;
                    }
                }

                fields.Add(startField);
            }

            return fields;
        }

        /// <summary>
        /// Getting a collection of ships, which players can buy by points.
        /// </summary>
        /// <returns>Collection of <see cref="IShip"/>, which players can buy by points</returns>
        public ICollection<ICommonShip> GetShips()
        {
            ICollection<ICommonShip> ships = new List<ICommonShip>();

            ships.Add(new Ship(ShipType.Auxiliary, 1, 100, 4));
            ships.Add(new Ship(ShipType.Mixed, 2, 200, 3));
            ships.Add(new Ship(ShipType.Mixed, 3, 300, 2));
            ships.Add(new Ship(ShipType.Military, 4, 400, 1));

            return ships;
        }

        /// <summary>
        /// Getting a collection of repairs, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IRepair"/>, which players can equip on the ship</returns>
        public ICollection<IRepair> GetRepairs()
        {
            ICollection<IRepair> repairs = new List<IRepair>();

            repairs.Add(new BasicRepair(40, 10));

            return repairs;
        }

        /// <summary>
        /// Getting a collection of weapons, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IWeapon"/>, which players can equip on the ship</returns>
        public ICollection<IWeapon> GetWeapons()
        {
            ICollection<IWeapon> weapons = new List<IWeapon>();

            weapons.Add(new BasicWeapon(50, 10));

            return weapons;
        }

        /// <summary>
        /// Creation and getting a new game ship.
        /// </summary>
        /// <param name="teamId">id of the team</param>
        /// <param name="ship">Type of <see cref="IShip"/>, which player wants to buy.</param>
        /// <returns><see cref="IGameShip"/> Game ship.</returns>
        public async Task<IGameShip> GetNewShipAsync(uint teamId, ICommonShip ship)
        {
            IGameShip gameShip = new GameShip(ship, teamId, GetShipCost(ship.Size));

            _repository.Ships.Create(ship);
            _repository.GameShips.Create(gameShip);
            await _repository.SaveAsync();

            return gameShip;
        }

        /// <summary>
        /// Add weapon to game ship.
        /// </summary>
        /// <param name="playerId">Id of team</param>
        /// <param name="gameShipId">Id of the game ship which adds a weapon.</param>
        /// <param name="weapon">A weapon (<see cref="IWeapon"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public async Task<StateCode> AddWeaponAsync(uint playerId, uint gameShipId, IWeapon weapon)
        {
            IGameShip gameShip = _repository.GameShips.Get(gameShipId);

            if (gameShip == null)
            {
                return StateCode.InvalidId;
            }

            if (gameShip.PlayerId != playerId)
            {
                return StateCode.InvalidTeam;
            }

            weapon = _repository.Weapons.Create(weapon);
            gameShip.Ship.Weapons.Add(weapon);

            await _repository.SaveAsync();

            return StateCode.Success;
        }

        /// <summary>
        /// Add repair to game ship.
        /// </summary>
        /// <param name="teamId">Id of team</param>
        /// <param name="gameShipId">Id of the game ship which adds a repair.</param>
        /// <param name="repair">A repair (<see cref="IRepair"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public async Task<StateCode> AddRepairAsync(uint teamId, uint gameShipId, IRepair repair)
        {
            IGameShip gameShip = _repository.GameShips.Get(gameShipId);

            if (gameShip == null)
            {
                return StateCode.InvalidId;
            }

            if (gameShip.PlayerId != teamId)
            {
                return StateCode.InvalidTeam;
            }

            repair = _repository.Repairs.Create(repair);
            gameShip.Ship.Repairs.Add(repair);

            await _repository.SaveAsync();

            return StateCode.Success;
        }

        /// <summary>
        /// Remove weapon from game ship.
        /// </summary>
        /// <param name="playerId">Id of team</param>
        /// <param name="gameShipId">Id of the game ship which removes a weapon.</param>
        /// <param name="weaponId">Id of weapon (<see cref="IWeapon"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public async Task<StateCode> RemoveWeaponAsync(uint playerId, uint gameShipId, uint weaponId)
        {
            IGameShip gameShip = _repository.GameShips.Get(gameShipId);
            IWeapon weapon = _repository.Weapons.Get(weaponId);

            if (gameShip == null || weapon == null)
            {
                return StateCode.InvalidId;
            }

            if (gameShip.PlayerId != playerId)
            {
                return StateCode.InvalidTeam;
            }

            gameShip.Ship.Weapons.Remove(weapon);
            _repository.Weapons.Delete(weapon.Id);

            await _repository.SaveAsync();

            return StateCode.Success;
        }

        /// <summary>
        /// Remove repair from game ship.
        /// </summary>
        /// <param name="playerId">Id of team</param>
        /// <param name="gameShipId">Id of the game ship which removes a repair.</param>
        /// <param name="repairId">Id of repair (<see cref="IRepair"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public async Task<StateCode> RemoveRepairAsync(uint playerId, uint gameShipId, uint repairId)
        {
            IGameShip gameShip = _repository.GameShips.Get(gameShipId);
            IRepair repair = _repository.Repairs.Get(repairId);

            if (gameShip == null || repair == null)
            {
                return StateCode.InvalidId;
            }

            if (gameShip.PlayerId != playerId)
            {
                return StateCode.InvalidTeam;
            }

            gameShip.Ship.Repairs.Remove(repair);
            _repository.Repairs.Delete(repair.Id);

            await _repository.SaveAsync();

            return StateCode.Success;
        }

        /// <summary>
        /// Getting border size of the game field.
        /// </summary>
        /// <returns><see cref="ILimitSize"/></returns>
        public ILimitSize GetLimitSizeField() =>
            new LimitSize(_maxSizeX, _maxSizeY, _minSizeX, _minSizeY);

        /// <summary>
        /// Create and add player to the game
        /// </summary>
        /// <param name="gameId">Game's id</param>
        /// <param name="playerName">Player's name</param>
        /// <returns><see cref="IPlayer"/> created player, otherwise null.</returns>
        public async Task<IPlayer> AddPlayerToGame(uint gameId, string playerName)
        {
            IGame game = _repository.Games.Get(gameId);

            if (game == null || game.CurrentCountPlayers == game.MaxNumberOfPlayers)
            {
                return null;
            }

            game.CurrentCountPlayers++;
            IPlayer player = _repository.Players.Create(new Player(playerName, gameId));
            game.PlayersId.Add(player.Id);

            await _repository.SaveAsync();

            return player;
        }

        /// <summary>
        /// Get start field by player and game. In case absence of starting fields, create them.
        /// </summary>
        /// <param name="gameId">Game's id of <see cref="IGame"/></param>
        /// <param name="playerId">Player's id of <see cref="IPlayer"/></param>
        /// <returns><see cref="IStartField"/> otherwise null</returns>
        public async Task<IStartField> GetStartField(uint gameId, uint playerId)
        {
            IGame game = _repository.Games.Get(gameId);
            IGameField gameField = _repository.GameFields.Get(game?.FieldId ?? 0);
            IPlayer player = _repository.Players.Get(playerId);

            if (game == null || gameField == null || player == null)
            {
                return null;
            }

            //Get startFields for current game
            ICollection<IStartField> startFields =
                _repository.StartFields.GetAll().Where(f => f.GameId == gameId).ToList();

            //Variable for result
            IStartField startField;

            //in the case haven't created startFields - create them
            if (startFields.Count == 0)
            {
                foreach (var labelField in GenerateStartFields(game.FieldId, game.MaxNumberOfPlayers))
                {
                    startFields.Add(_repository.StartFields.Create(
                        new StartField(gameField, CalculateStartPoints(labelField), gameId)
                        {
                            GameId = gameId,
                            FieldLabels = labelField
                        }));
                }

                //get first of start fields for the current player
                startField = startFields.FirstOrDefault();
            }
            //in the case bd have start field for current player and game return it
            else if((startField=startFields.FirstOrDefault(f => f.PlayerId==playerId))!=null)
            {
                return startField;
            }
            //otherwise get first of free start fields.
            else
            {
                startField = startFields.FirstOrDefault(f => f.Player == null);
            }

            //add current player to start field
            startField.PlayerId = playerId;
            startField.Player = player.Name;

            await _repository.SaveAsync();

            return startField;
        }

        /// <summary>
        /// Create <see cref="IGame"/> by numberOfTeams
        /// </summary>
        /// <param name="numberOfTeams">Number of team members in the game</param>
        /// <returns><see cref="uint"/> id of the created game</returns>
        /// <exception cref="ArgumentOutOfRangeException">A number of teams are out of possible values.</exception>
        public async Task<uint> CreateGameAsync(byte numberOfTeams)
        {
            if (numberOfTeams < 1 || numberOfTeams > _maxNumberOfTeams)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(numberOfTeams)} is out of range [0;{_maxNumberOfTeams}]");
            }

            IGame game = new Game();
            _repository.Games.Create(game);

            await _repository.SaveAsync();

            return game.Id;
        }

        /// <summary>
        /// Calculation ship cost by his size.
        /// </summary>
        /// <param name="size">Size (length) of ship.</param>
        /// <returns>Amount of points of cost ship.</returns>
        protected int GetShipCost(byte size) => size * PriceCoefficient;
    }
}
