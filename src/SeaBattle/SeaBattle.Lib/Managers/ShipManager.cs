using System;
using System.Linq;
using SeaBattle.Lib.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<ShipManager> _logger;

        private readonly GenericRepository<StartField> _startFieldRepository;

        private readonly GenericRepository<GameShip> _gameShipRepository;

        private readonly GenericRepository<Weapon> _weaponRepository;

        private readonly GenericRepository<Repair> _repairRepository;

        

        public ShipManager(IShipStorageUtility storageUtility, GenericRepository<StartField> startFieldRepository, GenericRepository<GameShip> gameShipRepository, GenericRepository<Weapon> weaponRepository, GenericRepository<Repair> repairRepository, ILogger<ShipManager> logger)
        {
            _startFieldRepository = startFieldRepository;
            _gameShipRepository = gameShipRepository;
            _weaponRepository = weaponRepository;
            _repairRepository = repairRepository;
            _logger = logger;
            _storageUtility = storageUtility;
        }

        public async Task<StateCode> BuyShip(GameShip gameShip, StartField startField)
        {
            if (gameShip.Points > startField.Points)
            {
                return StateCode.PointsShortage;
            }

            startField.GameShips.Add(gameShip);
            startField.Points -= gameShip.Points;

            await _gameShipRepository.CreateAsync(gameShip);

            try
            {
                await _startFieldRepository.UpdateAsync(s => s.Id == startField.Id, startField.GameShips,
                    _startFieldRepository.GetAll(), "GameShips");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error update data in database in progress {nameof(BuyShip)}", gameShip, startField);

                return StateCode.InvalidOperation;
            }

            return StateCode.Success;
        }

        public async Task<StateCode> SellShip(GameShip gameShip, StartField startField)
        {
            startField.GameShips.Remove(gameShip);
            startField.Points += gameShip.Points;

            try
            {
                await _gameShipRepository.DeleteAsync(gameShip);

                await _startFieldRepository.UpdateAsync(s => s.Id == startField.Id, startField.GameShips,
                    _startFieldRepository.GetAll(), "GameShips");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error update data in database in progress {nameof(SellShip)}", gameShip, startField);

                return StateCode.InvalidOperation;
            }

            return StateCode.Success;
        }

        public async Task<GameShip> GetNewShip(GamePlayer gamePlayer, Ship ship)
        {
            GameShip gameShip = new GameShip(ship, gamePlayer, _storageUtility.CalculatePointCost(ship.Size,ship.ShipTypeId));

            try
            {
                gameShip = await _gameShipRepository.CreateAsync(gameShip);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error create data in database in progress {nameof(GetNewShip)}", ship,
                    gameShip);

                return null;
            }

            switch (ship.ShipTypeId)
            {
                case 1:
                    Weapon weapon = _weaponRepository.FindById(1);

                    for (int i = 0; i < ship.Size; i++)
                    {
                        await AddWeapon(gameShip, weapon);
                    }

                    break;
                case 2:
                    break;
                case 3:
                    Repair repair = _repairRepository.FindById(1);

                    for (int i = 0; i < ship.Size; i++)
                    {
                        await AddRepair(gameShip, repair);
                    }

                    break;
                default:
                    Exception ex = new Exception("Wrong shipTypeId (not implemented)");
                    ex.Data.Add("Data", ship);

                    _logger.LogError(ex, $"Error add equipment to ship in progress {nameof(GetNewShip)}");

                    break;
            }

            return gameShip;
        }

        public async Task<StateCode> AddWeapon(GameShip gameShip, Weapon weapon)
        {

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if (gameShip.Ship.ShipTypeId == 3)
            {
                return StateCode.InvalidEquipment;
            }

            gameShip.Weapons.Add(weapon);

            try
            {
                await _gameShipRepository.UpdateAsync(s => s.Id == gameShip.Id, gameShip.Weapons,
                    _weaponRepository.GetAll(), "Weapons");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error update data in database in progress {nameof(AddWeapon)}", gameShip, weapon);

                return StateCode.InvalidOperation;
            }

            return StateCode.Success;
        }

        public async Task<StateCode> AddRepair(GameShip gameShip, Repair repair)
        {
            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if (gameShip.Ship.ShipTypeId == 1)
            {
                return StateCode.InvalidEquipment;
            }

            gameShip.Repairs.Add(repair);

            try
            {
                await _gameShipRepository.UpdateAsync(s => s.Id == gameShip.Id, gameShip.Repairs,
                    _repairRepository.GetAll(), "Repairs");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error update data in database in progress {nameof(AddRepair)}", gameShip, repair);

                return StateCode.InvalidOperation;
            }

            return StateCode.Success;
        }

        public async Task<StateCode> RemoveWeapon(GameShip gameShip, Weapon weapon)
        {
            EquippedWeapon eqWeapon = gameShip.EquippedWeapons.FirstOrDefault(w => w.WeaponId == weapon.Id);

            if (eqWeapon == null || !gameShip.EquippedWeapons.Remove(eqWeapon))
            {
                return StateCode.InvalidEquipment;
            }

            try
            {
                await _gameShipRepository.UpdateAsync(s => s.Id == gameShip.Id, gameShip.EquippedWeapons,
                    gameShip.EquippedWeapons, "EquippedWeapons");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error remove data from database in progress {nameof(RemoveWeapon)}", gameShip, weapon);

                return StateCode.InvalidOperation;
            }

            return StateCode.Success;
        }

        public async Task<StateCode> RemoveRepair(GameShip gameShip, Repair repair)
        {
            EquippedRepair eqRepair = gameShip.EquippedRepairs.FirstOrDefault(w => w.RepairId == repair.Id);

            if (eqRepair == null || !gameShip.EquippedRepairs.Remove(eqRepair))
            {
                return StateCode.InvalidEquipment;
            }

            try
            {
                await _gameShipRepository.UpdateAsync(s => s.Id == gameShip.Id, gameShip.EquippedRepairs,
                    gameShip.EquippedRepairs, "EquippedRepairs");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error remove data from database in progress {nameof(RemoveRepair)}", gameShip, repair);

                return StateCode.InvalidOperation;
            }

            return StateCode.Success;
        }
    }
}
