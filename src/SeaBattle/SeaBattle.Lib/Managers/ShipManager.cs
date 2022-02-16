using System;
using SeaBattle.Lib.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Repositories;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for creating and change ships, implements <see cref="IShipManager"/>.
    /// </summary>
    public class ShipManager : IShipManager
    {
        private readonly IShipStorageUtility _storageUtility;

        private readonly GenericRepository<StartField> _startFieldRepository;

        private readonly GenericRepository<GameShip> _gameShipRepository;

        private readonly GenericRepository<Weapon> _weaponRepository;

        private readonly GenericRepository<Repair> _repairRepository;

        public ShipManager(IShipStorageUtility storageUtility, GenericRepository<StartField> startFieldRepository, GenericRepository<GameShip> gameShipRepository, GenericRepository<Weapon> weaponRepository, GenericRepository<Repair> repairRepository)
        {
            _startFieldRepository = startFieldRepository;
            _gameShipRepository = gameShipRepository;
            _weaponRepository = weaponRepository;
            _repairRepository = repairRepository;
            _storageUtility = storageUtility;
        }

        public async Task<StateCode> BuyShip(ICollection<GamePlayer> players, GameShip gameShip, StartField startField)
        {
            if (gameShip.Points > startField.Points)
            {
                return StateCode.PointsShortage;
            }

            startField.GameShips.Add(gameShip);
            startField.Points -= gameShip.Points;

            await _gameShipRepository.CreateAsync(gameShip);

            await _startFieldRepository.UpdateAsync(s => s.Id == startField.Id, startField.GameShips,
                _startFieldRepository.GetAll(), "GameShips");

            return StateCode.Success;
        }

        public async Task<StateCode> SellShip(ICollection<GamePlayer> players, GameShip gameShip, StartField startField)
        {
            startField.GameShips.Remove(gameShip);
            startField.Points += gameShip.Points;

            await _gameShipRepository.DeleteAsync(gameShip);

            await _startFieldRepository.UpdateAsync(s => s.Id == startField.Id, startField.GameShips,
                _startFieldRepository.GetAll(), "GameShips");

            return StateCode.Success;
        }

        public async Task<GameShip> GetNewShip(GamePlayer gamePlayer, Ship ship)
        {
            GameShip gameShip = new GameShip(ship, gamePlayer, _storageUtility.CalculatePointCost(ship.Size,ship.ShipTypeId));
            switch (ship.ShipTypeId)
            {
                case 1:
                    Weapon weapon = _weaponRepository.FindById(1);
                    for (int i = 0; i < ship.Size; i++)
                    {
                        await AddWeapon(gamePlayer, gameShip, weapon);
                    }
                    break;
                case 2:
                    break;
                case 3:
                    Repair repair = _repairRepository.FindById(1);
                    for (int i = 0; i < ship.Size; i++)
                    {
                        await AddRepair(gamePlayer, gameShip, repair);
                    }
                    break;
                default:
                    Exception e = new Exception("Wrong shipTypeId (not implemented)");
                    e.Data.Add("Data", ship);
                    throw e;
            }

            return gameShip;
        }

        public async Task<StateCode> AddWeapon(GamePlayer gamePlayer, GameShip gameShip, Weapon weapon)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if (gameShip.Ship.ShipTypeId == 3)
            {
                return StateCode.InvalidEquipment;
            }

            gameShip.Weapons.Add(weapon);

            return StateCode.Success;
        }

        public async Task<StateCode> AddRepair(GamePlayer gamePlayer, GameShip gameShip, Repair repair)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if (gameShip.Ship.ShipTypeId == 1)
            {
                return StateCode.InvalidEquipment;
            }

            gameShip.Repairs.Add(repair);

            return StateCode.Success;
        }

        public StateCode RemoveWeapon(IGamePlayer gamePlayer, IGameShip gameShip, IWeapon weapon)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            return gameShip.Weapons.Remove(weapon as Weapon) ? StateCode.Success : StateCode.InvalidEquipment;
        }

        public StateCode RemoveRepair(IGamePlayer gamePlayer, IGameShip gameShip, IRepair repair)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            return gameShip.Repairs.Remove(repair as Repair) ? StateCode.Success : StateCode.InvalidEquipment;
        }
    }
}
