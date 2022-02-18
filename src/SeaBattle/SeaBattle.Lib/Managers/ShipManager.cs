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

        private readonly GenericRepository<GamePlayer> _gamePlayerRepository;

        private readonly GenericRepository<Ship> _shipRepository;

        public ShipManager(IShipStorageUtility storageUtility, GenericRepository<StartField> startFieldRepository, GenericRepository<GameShip> gameShipRepository, GenericRepository<Weapon> weaponRepository, GenericRepository<Repair> repairRepository, ILogger<ShipManager> logger, GenericRepository<GamePlayer> gamePlayerRepository, GenericRepository<Ship> shipRepository)
        {
            _startFieldRepository = startFieldRepository;
            _gameShipRepository = gameShipRepository;
            _weaponRepository = weaponRepository;
            _repairRepository = repairRepository;
            _logger = logger;
            _gamePlayerRepository = gamePlayerRepository;
            _shipRepository = shipRepository;
            _storageUtility = storageUtility;
        }

        public async Task<StateCode> BuyShip(int gameShipId, int startFieldId)
        {
            GameShip gameShip = await _gameShipRepository.FindByIdAsync(gameShipId);

            var query = await _startFieldRepository.GetWithIncludeAsync(f => f.Id == startFieldId, s => s.GameShips);
            StartField startField = query.FirstOrDefault();

            if (gameShip == null || startField == null)
            {
                _logger.LogWarning($"Invalid Id arguments in progress {nameof(BuyShip)}", gameShipId,
                    startFieldId);

                return StateCode.NullReference;
            }

            if (gameShip.Points > startField.Points)
            {
                return StateCode.PointsShortage;
            }

            startField.GameShips.Add(gameShip);
            startField.Points -= gameShip.Points;

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

        public async Task<StateCode> SellShip(int gameShipId, int startFieldId)
        {
            GameShip gameShip = await _gameShipRepository.FindByIdAsync(gameShipId);

            var query = await _startFieldRepository.GetWithIncludeAsync(f => f.Id == startFieldId, s => s.GameShips);
            StartField startField = query.FirstOrDefault();

            if (gameShip == null || startField == null)
            {
                _logger.LogWarning($"Invalid Id arguments in progress {nameof(SellShip)}", gameShipId,
                    startFieldId);

                return StateCode.NullReference;
            }

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

        public async Task<IGameShip> GetNewShip(string gamePlayerName, int shipId)
        {
            var query = await _gamePlayerRepository.GetAsync(p => p.Name == gamePlayerName);
            GamePlayer gamePlayer = query.FirstOrDefault();

            Ship ship = await _shipRepository.FindByIdAsync(shipId);

            if (gamePlayer == null || ship == null)
            {
                _logger.LogWarning($"Invalid Id arguments in progress {nameof(GetNewShip)}", shipId,
                    gamePlayerName);

                return null;
            }

            GameShip gameShip = new GameShip(ship, gamePlayer,
                _storageUtility.CalculatePointCost(ship.Size, ship.ShipType));

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

            switch ((int)ship.ShipType)
            {
                case 1:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        await AddWeapon(gameShip.Id, 1);
                    }

                    break;
                case 3:
                    break;
                case 2:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        await AddRepair(gameShip.Id, 1);
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

        public async Task<StateCode> AddWeapon(int gameShipId, int weaponId)
        {
            Weapon weapon = await _weaponRepository.FindByIdAsync(weaponId);

            var query = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == gameShipId, s => s.Weapons,
                s => s.Repairs, s => s.Ship);
            GameShip gameShip = query.FirstOrDefault();

            if (weapon == null || gameShip == null)
            {
                _logger.LogWarning($"Invalid Id arguments in progress {nameof(AddWeapon)}", gameShip,
                    weaponId);

                return StateCode.NullReference;
            }

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if ((int)gameShip.Ship.ShipType == 3)
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

        public async Task<StateCode> AddRepair(int gameShipId, int repairId)
        {
            Repair repair = await _repairRepository.FindByIdAsync(repairId);

            var query = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == gameShipId, s => s.Weapons,
                s => s.Repairs, s => s.Ship);
            GameShip gameShip = query.FirstOrDefault();

            if (repair == null || gameShip == null)
            {
                _logger.LogWarning($"Invalid Id arguments in progress {nameof(AddRepair)}", gameShipId,
                    repairId);

                return StateCode.NullReference;
            }

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if ((int)gameShip.Ship.ShipType == 1)
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

        public async Task<StateCode> RemoveWeapon(int gameShipId, int weaponId)
        {
            var query = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == gameShipId, s => s.Weapons,
                s => s.EquippedWeapons);
            GameShip gameShip = query.FirstOrDefault();

            if (gameShip == null)
            {
                _logger.LogWarning($"Invalid Id arguments in progress {nameof(RemoveWeapon)}", gameShipId);

                return StateCode.NullReference;
            }

            EquippedWeapon eqWeapon = gameShip.EquippedWeapons.FirstOrDefault(w => w.WeaponId == weaponId);

            if (eqWeapon == null || !gameShip.EquippedWeapons.Remove(eqWeapon))
            {
                return StateCode.InvalidEquipment;
            }

            try
            {
                await _gameShipRepository.UpdateAsync(s => s.Id == gameShipId, gameShip.EquippedWeapons,
                    gameShip.EquippedWeapons, "EquippedWeapons");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error remove data from database in progress {nameof(RemoveWeapon)}", gameShip, weaponId);

                return StateCode.InvalidOperation;
            }

            return StateCode.Success;
        }

        public async Task<StateCode> RemoveRepair(int gameShipId, int repairId)
        {
            var query = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == gameShipId, s => s.Repairs,
                s => s.EquippedRepairs);
            GameShip gameShip = query.FirstOrDefault();

            if (gameShip == null)
            {
                _logger.LogWarning($"Invalid Id arguments in progress {nameof(RemoveRepair)}", gameShipId);

                return StateCode.NullReference;
            }

            EquippedRepair eqRepair = gameShip.EquippedRepairs.FirstOrDefault(w => w.RepairId == repairId);

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
                _logger.LogWarning(ex, $"Error remove data from database in progress {nameof(RemoveRepair)}", gameShip, repairId);

                return StateCode.InvalidOperation;
            }

            return StateCode.Success;
        }
    }
}
