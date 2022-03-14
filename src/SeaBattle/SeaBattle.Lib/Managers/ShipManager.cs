using System;
using System.Linq;
using SeaBattle.Lib.Entities;
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

        private readonly GenericRepository<GamePlayer> _gamePlayerRepository;

        private readonly GenericRepository<Ship> _shipRepository;

        public ShipManager(IShipStorageUtility storageUtility, GenericRepository<StartField> startFieldRepository,
            GenericRepository<GameShip> gameShipRepository, GenericRepository<Weapon> weaponRepository,
            GenericRepository<Repair> repairRepository, GenericRepository<GamePlayer> gamePlayerRepository,
            GenericRepository<Ship> shipRepository)
        {
            _startFieldRepository = startFieldRepository;
            _gameShipRepository = gameShipRepository;
            _weaponRepository = weaponRepository;
            _repairRepository = repairRepository;
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
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(BuyShip)}");
                ex.Data.Add("gameShipId", gameShipId);
                ex.Data.Add("startFieldId", startFieldId);

                throw ex;
            }

            if (gameShip.Points > startField.Points)
            {
                return StateCode.PointsShortage;
            }

            startField.GameShips.Add(gameShip);
            startField.Points -= gameShip.Points;

            await _startFieldRepository.UpdateAsync(startField);

            /*await _startFieldRepository.UpdateAsync(s => s.Id == startField.Id, startField.GameShips,
                _startFieldRepository.GetAll(), "GameShips");*/

            return StateCode.Success;
        }

        public async Task<StateCode> SellShip(int gameShipId, int startFieldId)
        {
            GameShip gameShip = await _gameShipRepository.FindByIdAsync(gameShipId);

            var query = await _startFieldRepository.GetWithIncludeAsync(f => f.Id == startFieldId, s => s.GameShips);
            StartField startField = query.FirstOrDefault();

            if (gameShip == null || startField == null)
            {
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(SellShip)}");
                ex.Data.Add("gameShipId", gameShipId);
                ex.Data.Add("startFieldId", startFieldId);

                throw ex;
            }

            if (!startField.GameShips.Contains(gameShip))
            {
                Exception ex = new Exception($"StartField doesn't contain GameShip in progress {nameof(SellShip)}");
                ex.Data.Add("gameShipId", gameShipId);
                ex.Data.Add("startFieldId", startFieldId);

                throw ex;
            }

            startField.GameShips.Remove(gameShip);
            startField.Points += gameShip.Points;

            await _gameShipRepository.DeleteAsync(gameShip);

            await _startFieldRepository.UpdateAsync(startField);

            /*await _startFieldRepository.UpdateAsync(s => s.Id == startField.Id, startField.GameShips,
                _startFieldRepository.GetAll(), "GameShips");*/

            return StateCode.Success;
        }

        public async Task<IGameShip> GetNewShip(string gamePlayerName, int shipId)
        {
            var query = await _gamePlayerRepository.GetAsync(p => p.Name == gamePlayerName);
            GamePlayer gamePlayer = query.FirstOrDefault();

            Ship ship = await _shipRepository.FindByIdAsync(shipId);

            if (gamePlayer == null || ship == null)
            {
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(GetNewShip)}");
                ex.Data.Add("shipId", shipId);
                ex.Data.Add("gamePlayerName", gamePlayerName);

                throw ex;
            }

            GameShip gameShip = new GameShip(ship, gamePlayer,
                _storageUtility.CalculatePointCost(ship.Size, ship.ShipType));

            gameShip = await _gameShipRepository.CreateAsync(gameShip);

            switch (ship.ShipType)
            {
                case ShipType.Military:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        await AddWeapon(gameShip.Id, 1);
                    }

                    break;
                case ShipType.Mixed:
                    break;
                case ShipType.Auxiliary:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        await AddRepair(gameShip.Id, 1);
                    }

                    break;
                default:
                    Exception ex = new Exception($"Error add equipment to ship in progress {nameof(GetNewShip)}");
                    ex.Data.Add("Data", ship);

                    throw ex;
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
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(AddWeapon)}");
                ex.Data.Add("gameShip", gameShip);
                ex.Data.Add("weaponId", weaponId);

                throw ex;
            }

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if ((int) gameShip.Ship.ShipType == 3)
            {
                return StateCode.InvalidEquipment;
            }

            gameShip.Weapons.Add(weapon);

            await _gameShipRepository.UpdateAsync(gameShip);

            /*await _gameShipRepository.UpdateAsync(s => s.Id == gameShip.Id, gameShip.Weapons,
                _weaponRepository.GetAll(), "Weapons");*/

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
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(AddRepair)}");
                ex.Data.Add("gameShipId", gameShipId);
                ex.Data.Add("repairId", repairId);

                throw ex;
            }

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if ((int) gameShip.Ship.ShipType == 1)
            {
                return StateCode.InvalidEquipment;
            }

            gameShip.Repairs.Add(repair);

            await _gameShipRepository.UpdateAsync(gameShip);

            /*await _gameShipRepository.UpdateAsync(s => s.Id == gameShip.Id, gameShip.Repairs,
                _repairRepository.GetAll(), "Repairs");*/

            return StateCode.Success;
        }

        public async Task<StateCode> RemoveWeapon(int gameShipId, int weaponId)
        {
            var query = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == gameShipId, s => s.Weapons,
                s => s.EquippedWeapons);
            GameShip gameShip = query.FirstOrDefault();

            if (gameShip == null)
            {
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(RemoveWeapon)}");
                ex.Data.Add("gameShipId", gameShipId);

                throw ex;
            }

            EquippedWeapon eqWeapon = gameShip.EquippedWeapons.FirstOrDefault(w => w.WeaponId == weaponId);

            if (eqWeapon == null || !gameShip.EquippedWeapons.Remove(eqWeapon))
            {
                return StateCode.InvalidEquipment;
            }

            await _gameShipRepository.UpdateAsync(gameShip);

            /*await _gameShipRepository.UpdateAsync(s => s.Id == gameShipId, gameShip.EquippedWeapons,
                gameShip.EquippedWeapons, "EquippedWeapons");*/

            return StateCode.Success;
        }

        public async Task<StateCode> RemoveRepair(int gameShipId, int repairId)
        {
            var query = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == gameShipId, s => s.Repairs,
                s => s.EquippedRepairs);
            GameShip gameShip = query.FirstOrDefault();

            if (gameShip == null)
            {
                Exception ex = new Exception($"Invalid Id arguments in progress {nameof(RemoveRepair)}");
                ex.Data.Add("gameShipId", gameShipId);

                throw ex;
            }

            EquippedRepair eqRepair = gameShip.EquippedRepairs.FirstOrDefault(w => w.RepairId == repairId);

            if (eqRepair == null || !gameShip.EquippedRepairs.Remove(eqRepair))
            {
                return StateCode.InvalidEquipment;
            }

            await _gameShipRepository.UpdateAsync(gameShip);

            /*await _gameShipRepository.UpdateAsync(s => s.Id == gameShip.Id, gameShip.EquippedRepairs,
                gameShip.EquippedRepairs, "EquippedRepairs");*/

            return StateCode.Success;
        }
    }
}
