using SeaBattle.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Responses;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for creating and initializing start entities, implements <see cref="IInitializeManager"/>.
    /// </summary>
    public class InitializeManager : IInitializeManager
    {
        /// <summary>
        /// Price (amount of points) for 1 cell of ship
        /// </summary>
        private const int PriceCoefficient = 1000;

        /// <summary>
        /// Min size X of <see cref="IGameField"/>
        /// </summary>
        private ushort _minSizeX;

        /// <summary>
        /// Max size X of <see cref="IGameField"/>
        /// </summary>
        private ushort _maxSizeX;

        /// <summary>
        /// Min size Y of <see cref="IGameField"/>
        /// </summary>
        private ushort _minSizeY;

        /// <summary>
        /// Max size Y of <see cref="IGameField"/>
        /// </summary>
        private ushort _maxSizeY;

        /// <summary>
        /// Max number of players in <see cref="IGame"/>
        /// </summary>
        private byte _maxNumberOfPlayers;

        /// <summary>
        /// Count of created entities.
        /// </summary>
        private uint _entityCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeManager"/> class
        /// </summary>
        /// <param name="minSizeX">Min size X of <see cref="IGameField"/></param>
        /// <param name="maxSizeX">Max size X of <see cref="IGameField"/></param>
        /// <param name="minSizeY">Min size Y of <see cref="IGameField"/></param>
        /// <param name="maxSizeY">Max size Y of <see cref="IGameField"/></param>
        /// <param name="maxNumberOfPlayers">Max number of players for <see cref="IGame"/></param>
        public InitializeManager(ushort minSizeX, ushort maxSizeX, ushort minSizeY, ushort maxSizeY, byte maxNumberOfPlayers)
        {
            _minSizeX = minSizeX;
            _maxSizeX = maxSizeX;
            _minSizeY = minSizeY;
            _maxSizeY = maxSizeY;
            _maxNumberOfPlayers = maxNumberOfPlayers;
        }

        public IResponseGameField CreateGameField(ushort sizeX, ushort sizeY)
        {
            if (sizeX < _minSizeX || sizeX > _maxSizeX || sizeY < _minSizeY || sizeY > _maxSizeY)
            {
                return new ResponseGameField(null, StateCode.InvalidFieldSize);
            }

            GameField field = new GameField(sizeX, sizeY, ++_entityCount);

            return new ResponseGameField(field, StateCode.Success);
        }

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
        /// <returns>false - there is ship around target cell, otherwise true - around area is free.</returns>
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

        public ICollection<bool[,]> GenerateStartFields(IGameField gameField, byte numberOfPlayers)
        {
            if (numberOfPlayers > _maxNumberOfPlayers || numberOfPlayers <= 0)
            {
                throw new ArgumentOutOfRangeException("Wrong value of the number of players");
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

        public LimitSize GetLimitSizeField() =>
            new LimitSize(_maxSizeX, _maxSizeY, _minSizeX, _minSizeY);

        public IResponseGamePlayer AddPlayerToGame(IGame game, string playerName)
        {
            if (game.CurrentCountPlayers == game.MaxNumberOfPlayers)
            {
                return new ResponseGamePlayer(null, StateCode.ExceededMaxNumberOfPlayers);
            }

            IGamePlayer player = new GamePlayer(++_entityCount, playerName);

            game.Players.Add(player);

            return new ResponseGamePlayer(player, StateCode.Success);
        }

        public IResponseStartField GetStartField(IGame game, IGamePlayer gamePlayer)
        {
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
                catch (ArgumentOutOfRangeException)
                {
                    return new ResponseStartField(null, StateCode.ExceededMaxNumberOfPlayers);
                }
                foreach (var labelField in fieldsOfLabels)
                {
                    game.StartFields.Add(
                        new StartField(++_entityCount, game.Field, labelField, gamePlayer, CalculateStartPoints(labelField), new List<IGameShip>(), game.Id)
                        {
                            FieldLabels = labelField
                        });
                }

                //get first of start fields for the current player
                startField = game.StartFields.FirstOrDefault();
            }
            //in the case bd have start field for current player and game return it
            else if ((startField = game.StartFields.FirstOrDefault(f => f.GamePlayer == gamePlayer)) != null)
            {
                return new ResponseStartField(startField, StateCode.Success);
            }
            //otherwise get first of free start fields.
            else
            {
                startField = game.StartFields.FirstOrDefault(f => f.GamePlayer == null);
            }

            //add current player to start field
            if (startField != null)
            {
                startField.GamePlayer = gamePlayer;

                //change status player
                startField.GamePlayer.State = PlayerState.InitializeField;

                return new ResponseStartField(startField, StateCode.Success);
            }

            return new ResponseStartField(null, StateCode.InvalidPlayer);
        }

        public IGame CreateGame(byte numberOfPlayers)
        {
            if (numberOfPlayers < 1 || numberOfPlayers > _maxNumberOfPlayers)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(numberOfPlayers)} is out of range [0;{_maxNumberOfPlayers}]");
            }

            return new Game(numberOfPlayers) {Id = ++_entityCount};
        }

        /// <summary>
        /// Calculation ship cost by his size.
        /// </summary>
        /// <param name="size">Size (length) of ship.</param>
        /// <returns>Amount of points of cost ship.</returns>
        protected int GetShipCost(byte size) => size * PriceCoefficient;
    }
}
