using SeaBattle.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    public class InitializeManager
    {
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

        public IResponseGameField CreateGameField(ushort sizeX, ushort sizeY)
        {
            if (sizeX < _minSizeX || sizeX > _maxSizeX || sizeY < _minSizeY || sizeY > _maxSizeY)
            {
                return new ResponseGameField(null, StateCode.InvalidFieldSize);
            }

            GameField field = new GameField(sizeX, sizeY);
            _repository.GameFields.Create(field);

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
            return countShips * 1000;
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

        public async Task<StateCode> BuyShip(uint teamId, uint gameShipId, uint startFieldId)
        {
            IStartField field = _repository.StartFields.Get(startFieldId);
            ITeam team = _repository.Teams.Get(teamId);
            IGameShip ship = _repository.GameShips.Get(gameShipId);

            if (field == null || team == null || ship == null)
            {
                return StateCode.InvalidId;
            }

            if (field.Team != team.Name)
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

        public async Task<StateCode> SellShip(uint teamId, uint gameShipId, uint startFieldId)
        {
            IStartField field = _repository.StartFields.Get(startFieldId);
            ITeam team = _repository.Teams.Get(teamId);
            IGameShip ship = _repository.GameShips.Get(gameShipId);

            if (field == null || team == null || ship == null)
            {
                return StateCode.InvalidId;
            }

            if (field.Team != team.Name)
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
    }
}
