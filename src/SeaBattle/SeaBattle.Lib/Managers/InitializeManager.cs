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
                return new ResponseGameField(null, StateCode.InvalidFieldSize);
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
        /// <param name="field">Field with ships - array with type bool, where true - placed boat, false - empty.</param>
        /// <param name="x">Coordinate X where placed ship which we relatively check free area.</param>
        /// <param name="y">Coordinate Y where placed ship which we relatively check free area.</param>
        /// <returns>Result of check: true - there is ship around target cell, false - around area is free.</returns>
        private bool CheckFreeAreaAroundShip(bool[,] field, int x, int y)
        {
            int sizeX = field.GetLength(0);
            int sizeY = field.GetLength(1);
            if (x < 0 || x >= sizeX || y < 0 || y > sizeY)
                throw new ArgumentOutOfRangeException();
            int offsetX = -1;
            int offsetY = 0;
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    //Check free cells on diagonal from the cell with coordinates x,y
                    if (field[x + Convert.ToInt32(Math.Pow(-1, i / 2)), x + Convert.ToInt32(Math.Pow(-1, (i + 1) / 2))] == true)
                        return false;
                }
                //In case of exception, we hit the border which means the cell is the fulfilled condition
                catch (Exception e) {}
                try
                {
                    //Check free cells on horizontal and vertical from the cell with coordinates x,y
                    if (field[x + offsetX, x + offsetY] == true)
                        return false;
                }
                //In case of exception, we hit the border which means the cell is the fulfilled condition
                catch (Exception e) { }
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
            if (field.Team != team.Name)
                return StateCode.InvalidTeam;
            if (ship.Points > field.Points)
                return StateCode.PointsShortage;
            field.Ships.Add(ship);
            field.Points -= ship.Points;
            _repository.StartFields.Update(field);
            await _repository.Save();
            return StateCode.Success;
        }

        public async Task<StateCode> SellShip(uint teamId, uint gameShipId, uint startFieldId)
        {
            IStartField field = _repository.StartFields.Get(startFieldId);
            ITeam team = _repository.Teams.Get(teamId);
            IGameShip ship = _repository.GameShips.Get(gameShipId);
            if (field.Team != team.Name)
                return StateCode.InvalidTeam;
            field.Ships.Remove(ship);
            field.Points += ship.Points;
            _repository.Ships.Delete(ship.Ship.Id);
            _repository.GameShips.Delete(ship.Id);
            _repository.StartFields.Update(field);
            await _repository.Save();
            return StateCode.Success;
        }
    }
}
