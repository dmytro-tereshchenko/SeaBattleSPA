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

        private ushort _minSizeX;

        private ushort _maxSizeX;

        private ushort _minSizeY;

        private ushort _maxSizeY;

        private byte _maxNumberOfPlayers;

        public InitializeManager(ushort minSizeX, ushort maxSizeX, ushort minSizeY, ushort maxSizeY, byte maxNumberOfPlayers)
        {
            _minSizeX = minSizeX;
            _maxSizeX = maxSizeX;
            _minSizeY = minSizeY;
            _maxSizeY = maxSizeY;
            _maxNumberOfPlayers = maxNumberOfPlayers;
        }

        /// <summary>
        /// Creating and getting game field.
        /// </summary>
        /// <param name="sizeX">Size X of created game field.</param>
        /// <param name="sizeY">Size Y of created game field.</param>
        /// <returns><see cref="IResponseGameField"/> where Value is <see cref="IGameField"/>, State is <see cref="StateCode"/></returns>
        public IResponseGameField CreateGameField(ushort sizeX, ushort sizeY)
        {
            if (sizeX < _minSizeX || sizeX > _maxSizeX || sizeY < _minSizeY || sizeY > _maxSizeY)
            {
                return new ResponseGameField(null, StateCode.InvalidFieldSize);
            }

            GameField field = new GameField(sizeX, sizeY);

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
        /// <param name="players">Collection of players in the game</param>
        /// <param name="gameShip">Game ship</param>
        /// <param name="startField">Start field with initializing data and parameters for the player</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode BuyShip(ICollection<IPlayer> players, IGameShip gameShip, IStartField startField)
        {
            if (!players.Contains(startField.Player))
            {
                return StateCode.InvalidPlayer;
            }

            if (gameShip.Points > startField.Points)
            {
                return StateCode.PointsShortage;
            }

            startField.Ships.Add(gameShip);
            startField.Points -= gameShip.Points;


            return StateCode.Success;
        }

        /// <summary>
        /// Sell ship and remove from <see cref="IStartField"/>
        /// </summary>
        /// <param name="players">Collection of players in the game</param>
        /// <param name="gameShip">Game ship</param>
        /// <param name="startField">Start field with initializing data and parameters for the player</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        public StateCode SellShip(ICollection<IPlayer> players, IGameShip gameShip, IStartField startField)
        {
            if (!players.Contains(startField.Player))
            {
                return StateCode.InvalidPlayer;
            }

            startField.Ships.Remove(gameShip);
            startField.Points += gameShip.Points;

            return StateCode.Success;
        }

        /// <summary>
        /// Method for generating a collection of fields with labels for possible placing ships on start field for teams.
        /// </summary>
        /// <param name="gameField">Field of the game</param>
        /// <param name="numberOfPlayers">Amount of players</param>
        /// <returns>Collection of fields with ships - arrays with type bool, where true - placed boat, false - empty</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="numberOfPlayers"/> out of range</exception>
        /// <exception cref="ArgumentNullException">There is no gaming field for the given <paramref name="gameField"/>.</exception>
        public ICollection<bool[,]> GenerateStartFields(IGameField gameField, byte numberOfPlayers)
        {
            if (numberOfPlayers > _maxNumberOfPlayers || numberOfPlayers <= 0)
            {
                throw new ArgumentOutOfRangeException("Wrong value of the number of players");
            }

            if (gameField == null)
            {
                throw new ArgumentNullException();
            }

            //List for result of method
            ICollection<bool[,]> fields = new List<bool[,]>(numberOfPlayers);

            //Amount of rows and columns for the position of teams.
            int colTeam = (int)Math.Ceiling(Math.Sqrt((double)numberOfPlayers));
            int rowTeam = (int)Math.Ceiling((double)numberOfPlayers / colTeam);

            //Coordinates of begin and end every row and column of quadrant for every team. 
            (int begin, int end)[] borderQuadrantsX = new (int, int)[rowTeam];
            (int begin, int end)[] borderQuadrantsY = new (int, int)[colTeam];

            //Calculation begins and ends of quadrants.
            for (int i = 0; i < rowTeam; i++)
            {
                borderQuadrantsX[i].begin = i == 0 ? 0 : i * gameField.SizeX / rowTeam + 1;
                borderQuadrantsX[i].end = i + 1 == rowTeam ? gameField.SizeX - 1 : (i + 1) * gameField.SizeX / rowTeam - 1;
            }

            for (int i = 0; i < colTeam; i++)
            {
                borderQuadrantsY[i].begin = i == 0 ? 0 : i * gameField.SizeY / colTeam + 1;
                borderQuadrantsY[i].end = i + 1 == colTeam ? gameField.SizeY - 1 : (i + 1) * gameField.SizeY / colTeam - 1;
            }

            //Coordinates of quadrant for the team.
            int quadrantX, quadrantY;

            //Add fields until the amount of fields = number of teams.
            while (fields.Count < numberOfPlayers)
            {
                //Calculation of coordinates of quadrant for the team.
                quadrantX = fields.Count / colTeam;
                quadrantY = fields.Count - quadrantX * colTeam;

                bool[,] startField = new bool[gameField.SizeX, gameField.SizeY];

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
        /// <returns>Collection of <see cref="ICommonShip"/>, which players can buy by points</returns>
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
        /// <param name="player">Player(user) in game</param>
        /// <param name="ship">Type of <see cref="ICommonShip"/>, which player wants to buy.</param>
        /// <returns><see cref="IGameShip"/> Game ship.</returns>
        public IGameShip GetNewShip(IPlayer player, ICommonShip ship) =>
            new GameShip(ship, player, GetShipCost(ship.Size));

        /// <summary>
        /// Add weapon to game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which adds a weapon.</param>
        /// <param name="weapon">A weapon (<see cref="IWeapon"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode AddWeapon(IPlayer player, IGameShip gameShip, IWeapon weapon)
        {
            if (gameShip == null || weapon == null || player == null)
            {
                return StateCode.NullReference;
            }

            if (gameShip.Player != player)
            {
                return StateCode.InvalidPlayer;
            }

            gameShip.Ship.Weapons.Add(weapon);

            return StateCode.Success;
        }

        /// <summary>
        /// Add repair to game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which adds a repair.</param>
        /// <param name="repair">A repair (<see cref="IRepair"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode AddRepair(IPlayer player, IGameShip gameShip, IRepair repair)
        {
            if (gameShip == null || player == null || repair == null)
            {
                return StateCode.NullReference;
            }

            if (gameShip.Player != player)
            {
                return StateCode.InvalidPlayer;
            }

            gameShip.Ship.Repairs.Add(repair);

            return StateCode.Success;
        }

        /// <summary>
        /// Remove weapon from game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which removes a weapon.</param>
        /// <param name="weapon">Weapon (<see cref="IWeapon"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode RemoveWeapon(IPlayer player, IGameShip gameShip, IWeapon weapon)
        {
            if (gameShip == null || weapon == null || player == null)
            {
                return StateCode.NullReference;
            }

            if (gameShip.Player != player)
            {
                return StateCode.InvalidPlayer;
            }

            gameShip.Ship.Weapons.Remove(weapon);

            return StateCode.Success;
        }

        /// <summary>
        /// Remove repair from game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which removes a repair.</param>
        /// <param name="repair">Repair (<see cref="IRepair"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode RemoveRepair(IPlayer player, IGameShip gameShip, IRepair repair)
        {

            if (gameShip == null || repair == null || player == null)
            {
                return StateCode.NullReference;
            }

            if (gameShip.Player != player)
            {
                return StateCode.InvalidPlayer;
            }

            gameShip.Ship.Repairs.Remove(repair);

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
        /// <param name="game">Current game</param>
        /// <param name="playerName">Player's name</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        public StateCode AddPlayerToGame(IGame game, string playerName)
        {
            if (game == null || string.IsNullOrWhiteSpace(playerName))
            {
                return StateCode.NullReference;
            }

            if (game.CurrentCountPlayers == game.MaxNumberOfPlayers)
            {
                return StateCode.ExceededMaxNumberOfPlayers;
            }

            game.CurrentCountPlayers++;
            game.Players.Add(new Player(playerName));

            return StateCode.Success;
        }

        /// <summary>
        /// Get start field by player and game. In case absence of starting fields, create them.
        /// </summary>
        /// <param name="game">Current game</param>
        /// <param name="player">Current player</param>
        /// <returns><see cref="IStartField"/> otherwise null</returns>
        public IStartField GetStartField(IGame game, IPlayer player)
        {
            if (game == null || player == null)
            {
                return null;
            }

            //Variable for result
            IStartField startField;

            //in the case haven't created startFields - create them
            if (game.StartFields.Count == 0)
            {
                ICollection<bool[,]> fieldsOfLabels;
                try
                {
                    fieldsOfLabels = GenerateStartFields(game.Field, game.MaxNumberOfPlayers);
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return null;
                }
                foreach (var labelField in fieldsOfLabels)
                {
                    game.StartFields.Add(
                        new StartField(game.Field, labelField, player, CalculateStartPoints(labelField), new List<IGameShip>(), game.Id)
                        {
                            FieldLabels = labelField
                        });
                }

                //get first of start fields for the current player
                startField = game.StartFields.FirstOrDefault();
            }
            //in the case bd have start field for current player and game return it
            else if ((startField = game.StartFields.FirstOrDefault(f => f.Player == player)) != null)
            {
                return startField;
            }
            //otherwise get first of free start fields.
            else
            {
                startField = game.StartFields.FirstOrDefault(f => f.Player == null);
            }

            //add current player to start field
            if (startField != null)
            {
                startField.Player = player;
            }

            return startField;
        }

        /// <summary>
        /// Create <see cref="IGame"/> by <paramref name="numberOfPlayers"/>
        /// </summary>
        /// <param name="numberOfPlayers">Number of players in the game</param>
        /// <returns><see cref="IGame"/> Created game</returns>
        /// <exception cref="ArgumentOutOfRangeException">A number of teams are out of possible values.</exception>
        public IGame CreateGame(byte numberOfPlayers)
        {
            if (numberOfPlayers < 1 || numberOfPlayers > _maxNumberOfPlayers)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(numberOfPlayers)} is out of range [0;{_maxNumberOfPlayers}]");
            }

            return new Game(numberOfPlayers);
        }

        /// <summary>
        /// Calculation ship cost by his size.
        /// </summary>
        /// <param name="size">Size (length) of ship.</param>
        /// <returns>Amount of points of cost ship.</returns>
        protected int GetShipCost(byte size) => size * PriceCoefficient;
    }
}
