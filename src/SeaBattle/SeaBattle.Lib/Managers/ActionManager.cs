using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Repositories;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// The manager responsible for the actions and changes of ships on the field during the game, implements <see cref="IActionManager"/>.
    /// </summary>
    public class ActionManager : IActionManager
    {
        public IGameFieldActionUtility ActionUtility { get; protected set; }

        private readonly GenericRepository<StartField> _startFieldRepository;

        private readonly GenericRepository<GameShip> _gameShipRepository;

        private readonly GenericRepository<GameField> _gameFieldRepository;

        private readonly GenericRepository<Game> _gameRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionManager"/> class
        /// </summary>
        /// <param name="actionUtility">Utility for actions on <see cref="IGameField"/></param>
        public ActionManager(IGameFieldActionUtility actionUtility,
            GenericRepository<StartField> startFieldRepository,
            GenericRepository<GameShip> gameShipRepository,
            GenericRepository<GameField> gameFieldRepository,
            GenericRepository<Game> gameRepository)
        {
            ActionUtility = actionUtility;
            _startFieldRepository = startFieldRepository;
            _gameShipRepository = gameShipRepository;
            _gameFieldRepository = gameFieldRepository;
            _gameRepository = gameRepository;
        }

        #region Getting data from GameField

        public IDictionary<IGameShip, ICollection<(ushort, ushort)>> GetFieldWithShips(IGameField field, IGamePlayer player = null)
        {
            //Search coordinates of ships
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> ships =
                ActionUtility.GetAllShipsCoordinates(field, player.Name);

            (float, float) centerField = (field.SizeX / 2, field.SizeY / 2);

            //sorting ships by distance from the center of the field
            return ships
                .OrderBy(s =>
                    ActionUtility.GetDistanceBetween2Points(centerField,
                        ActionUtility.GetGeometricCenterOfShip(s.Value)))
                .ToDictionary(s=>s.Key, s=>s.Value);
        }

        /// <summary>
        /// Get <see cref="ICollection{T}"/> of <see cref="IGameShip"/> on distance of action.
        /// </summary>
        /// <param name="playerName">Current playe's namer</param>
        /// <param name="ship">Current ship</param>
        /// <param name="field">Game field</param>
        /// <param name="action">Type of possible actions (<see cref="ActionType.Attack"/>, <see cref="ActionType.Repair"/>)</param>
        /// <returns><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"/></returns>
        /// <exception cref="ArgumentException">Wrong player</exception>
        /// <exception cref="InvalidEnumArgumentException">Used action not planned by the game</exception>
        private ICollection<IGameShip> GetVisibleTargetsForShip(string playerName, IGameShip ship, IGameField field,
            ActionType action)
        {
            if (!ship.GamePlayer.Name.Equals(playerName))
            {
                throw new ArgumentException($"Wrong ship for player {playerName}");
            }

            //Search coordinates of ships
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> ships = ActionUtility.GetAllShipsCoordinates(field, playerName);

            //Coordinates of geometric center for current ship
            (float, float) centerOfCurrentShip = ActionUtility.GetGeometricCenterOfShip(ships[ship]);

            ushort distance = action switch
            {
                ActionType.Attack => ship.AttackRange,
                ActionType.Repair => ship.RepairRange,
                _ => throw new InvalidEnumArgumentException()
            };

            //Filtering by distance
            var filteringShips = ships
                .Where(s => ActionUtility.GetDistanceBetween2Points(centerOfCurrentShip,
                    ActionUtility.GetGeometricCenterOfShip(s.Value)) <= distance)
                .Select(s => s.Key);

            //Filtering by player   (attack -> only not friendly targets;
            //                      repair -> only friendly targets)
            filteringShips = action switch
            {
                ActionType.Attack => filteringShips.Where(s => !s.GamePlayer.Name.Equals(playerName)),
                ActionType.Repair => filteringShips.Where(s => s.GamePlayer.Name.Equals(playerName)),
                _ => throw new InvalidEnumArgumentException()
            };

            return filteringShips.ToList();
        }

        #endregion

        #region Actions with StartField

        public async Task<StateCode> TransferShipFromGameField(string playerName, int shipId, int startFieldId)
        {
            var queryShip = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == shipId, s => s.GamePlayer, s => s.Ship);

            GameShip ship = queryShip.FirstOrDefault();

            var queryField = await _startFieldRepository.GetWithIncludeAsync(f => f.Id == startFieldId,
                f => f.GamePlayer,
                f => f.GameShips,
                f => f.GameField,
                f => f.GameField.GameFieldCells);

            StartField startField = queryField.FirstOrDefault();

            if (ship is null)
            {
                return StateCode.AbsentOfShip;
            }

            if (startField is null)
            {
                return StateCode.NullReference;
            }

            if (!ship.GamePlayer.Name.Equals(playerName) || !startField.GamePlayer.Name.Equals(playerName))
            {
                return StateCode.InvalidPlayer;
            }

            startField.GameShips.Add(ship);
            ActionUtility.RemoveShipFromField(ship, startField.GameField);

            await _startFieldRepository.UpdateAsync(startField);

            return StateCode.Success;
        }

        public async Task<StateCode> TransferShipToGameField(string playerName, ushort tPosX, ushort tPosY,
            DirectionOfShipPosition direction, int startFieldId, int shipId)
        {
            var queryShip = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == shipId, s => s.GamePlayer, s => s.Ship);

            GameShip ship = queryShip.FirstOrDefault();

            var queryField = await _startFieldRepository.GetWithIncludeAsync(f => f.Id == startFieldId,
                f => f.GamePlayer,
                f => f.GameShips,
                f => f.GameField,
                f => f.GameField.GameFieldCells);

            StartField startField = queryField.FirstOrDefault();

            Dictionary<int, GameShip> ships = new Dictionary<int, GameShip>();

            foreach (var cell in startField.GameField.GameFieldCells)
            {
                if (!ships.ContainsKey(cell.GameShipId))
                {
                    var queryShip2 = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == cell.GameShipId, s => s.Ship);
                    ships[cell.GameShipId] = queryShip2.FirstOrDefault();
                }
            }

            if (!startField.GameShips.Contains(ship))
            {
                return StateCode.InvalidShip;
            }

            if (!ship.GamePlayer.Name.Equals(playerName) || !startField.GamePlayer.Name.Equals(playerName))
            {
                return StateCode.InvalidPlayer;
            }

            StateCode result;
            try
            {
                result = ActionUtility.PutShipOnField(playerName, ship, tPosX, tPosY, direction, startField.GameField);
            }
            catch (InvalidEnumArgumentException)
            {
                result = StateCode.InvalidOperation;
            }

            if (result == StateCode.Success)
            {
                startField.GameShips.Remove(ship as GameShip);

                await _startFieldRepository.UpdateAsync(startField);
            }

            return result;
        }

        #endregion

        #region Actions with ship

        public async Task<StateCode> MoveShip(string playerName, int gameShipId, ushort tPosX, ushort tPosY,
            DirectionOfShipPosition direction, int gameFieldId)
        {
            var queryShip = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == gameShipId,
                s => s.GamePlayer,
                s => s.Ship);

            GameShip ship = queryShip.FirstOrDefault();

            var queryField = await _gameFieldRepository.GetWithIncludeAsync(f => f.Id == gameFieldId,
                f => f.GameFieldCells);

            GameField gameField = queryField.FirstOrDefault();

            Dictionary<int, GameShip> ships = new Dictionary<int, GameShip>();

            foreach (var cell in gameField.GameFieldCells)
            {
                if (!ships.ContainsKey(cell.GameShipId))
                {
                    var queryShip2 = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == cell.GameShipId, s => s.Ship);
                    ships[cell.GameShipId] = queryShip2.FirstOrDefault();
                }
            }

            if (!ship.GamePlayer.Name.Equals(playerName))
            {
                return StateCode.InvalidPlayer;
            }

            //Ship's coordinates on game field
            ICollection<(ushort, ushort)> locOfShip = ActionUtility.GetShipCoordinates(ship, gameField);

            //check distance between target point and the nearest cell of current ship.

            if (locOfShip.Select(s => ActionUtility.GetDistanceBetween2Points((tPosX, tPosY), s)).OrderBy(d => d).First() >
                ship.Speed)
            {
                return StateCode.OutOfDistance;
            }

            //clear previous position of ship
            foreach (var loc in locOfShip)
            {
                gameField[loc.Item1, loc.Item2] = null;
            }

            StateCode result;

            try
            {
                result = ActionUtility.PutShipOnField(playerName, ship, tPosX, tPosY, direction, gameField);
            }
            catch (InvalidEnumArgumentException)
            {
                result = StateCode.InvalidOperation;
            }

            if (result != StateCode.Success)
            {
                //if can't place the ship in the new position, then restore the old position of the ship
                foreach (var loc in locOfShip)
                {
                    gameField[loc.Item1, loc.Item2] = ship;
                }
            }

            await _gameFieldRepository.UpdateAsync(gameField);

            return result;
        }

        public async Task<StateCode> AttackShip(string playerName, int gameShipId, ushort tPosX, ushort tPosY, int gameFieldId)
        {
            var queryShip = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == gameShipId,
                s => s.GamePlayer,
                s => s.Ship,
                s => s.Weapons,
                s => s.EquippedWeapons);

            GameShip ship = queryShip.FirstOrDefault();

            var queryField = await _gameFieldRepository.GetWithIncludeAsync(f => f.Id == gameFieldId,
                f => f.GameFieldCells);

            GameField gameField = queryField.FirstOrDefault();

            Dictionary<int, GameShip> ships = new Dictionary<int, GameShip>();

            foreach (var cell in gameField.GameFieldCells)
            {
                if (!ships.ContainsKey(cell.GameShipId))
                {
                    var queryShip2 = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == cell.GameShipId, s => s.GamePlayer, s => s.Ship);
                    ships[cell.GameShipId] = queryShip2.FirstOrDefault();
                }
            }

            if (!ship.GamePlayer.Name.Equals(playerName))
            {
                return StateCode.InvalidPlayer;
            }

            if (gameField[tPosX, tPosY] is null || gameField[tPosX, tPosY].GamePlayer.Id == ship.GamePlayer.Id)
            {
                return StateCode.MissTarget;
            }

            GameShip targetShip = gameField[tPosX, tPosY];

            (float, float) centerOfTargetPoint = ((float) tPosX - 0.5f, (float) tPosY - 0.5f);

            if (ActionUtility.GetDistanceBetween2Points(centerOfTargetPoint,
                    ActionUtility.GetGeometricCenterOfShip(ActionUtility.GetShipCoordinates(ship, gameField))) >
                ship.AttackRange)
            {
                return StateCode.OutOfDistance;
            }

            if (targetShip.Hp <= ship.Damage)
            {
                //ActionUtility.RemoveShipFromField(targetShip, gameField);

                await _gameShipRepository.DeleteAsync(targetShip);

                return StateCode.TargetDestroyed;
            }

            targetShip.Hp -= ship.Damage;

            await _gameShipRepository.UpdateAsync(targetShip);

            return StateCode.Success;
        }

        public async Task<StateCode> RepairShip(string playerName, int gameShipId, ushort tPosX, ushort tPosY, int gameFieldId)
        {
            var queryShip = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == gameShipId,
                s => s.GamePlayer,
                s => s.Ship,
                s => s.Repairs,
                s => s.EquippedRepairs);

            GameShip ship = queryShip.FirstOrDefault();

            var queryField = await _gameFieldRepository.GetWithIncludeAsync(f => f.Id == gameFieldId,
                f => f.GameFieldCells);

            GameField gameField = queryField.FirstOrDefault();

            Dictionary<int, GameShip> ships = new Dictionary<int, GameShip>();

            foreach (var cell in gameField.GameFieldCells)
            {
                if (!ships.ContainsKey(cell.GameShipId))
                {
                    var queryShip2 = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == cell.GameShipId, s => s.GamePlayer, s => s.Ship);
                    ships[cell.GameShipId] = queryShip2.FirstOrDefault();
                }
            }

            if (!ship.GamePlayer.Name.Equals(playerName))
            {
                return StateCode.InvalidPlayer;
            }

            if (gameField[tPosX, tPosY] is null || gameField[tPosX, tPosY].GamePlayer.Id != ship.GamePlayer.Id)
            {
                return StateCode.MissTarget;
            }

            (float, float) centerOfTargetPoint = ((float)tPosX - 0.5f, (float)tPosY - 0.5f);

            if (ActionUtility.GetDistanceBetween2Points(centerOfTargetPoint,
                    ActionUtility.GetGeometricCenterOfShip(ActionUtility.GetShipCoordinates(ship, gameField))) >
                ship.RepairRange)
            {
                return StateCode.OutOfDistance;
            }

            GameShip targetShip = gameField[tPosX, tPosY];

            targetShip.Hp += ship.RepairPower;
            if (targetShip.Hp > targetShip.MaxHp)
            {
                targetShip.Hp = targetShip.MaxHp;
            }

            Game game = await _gameRepository.FindByIdAsync(gameField.GameId);

            bool endGame = CheckEndGame(game);

            await _gameShipRepository.UpdateAsync(targetShip);

            return endGame ? StateCode.GameFinished : StateCode.Success;
        }

        public async Task<StateCode> RepairAllShip(string playerName, int gameShipId, int gameFieldId)
        {
            var queryShip = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == gameShipId,
                s => s.GamePlayer,
                s => s.Ship,
                s => s.Repairs,
                s => s.EquippedRepairs);

            GameShip ship = queryShip.FirstOrDefault();

            var queryField = await _gameFieldRepository.GetWithIncludeAsync(f => f.Id == gameFieldId,
                f => f.GameFieldCells);

            GameField gameField = queryField.FirstOrDefault();

            Dictionary<int, GameShip> ships = new Dictionary<int, GameShip>();

            foreach (var cell in gameField.GameFieldCells)
            {
                if (!ships.ContainsKey(cell.GameShipId))
                {
                    var queryShip2 = await _gameShipRepository.GetWithIncludeAsync(s => s.Id == cell.GameShipId, s => s.GamePlayer, s => s.Ship);
                    ships[cell.GameShipId] = queryShip2.FirstOrDefault();
                }
            }

            if (!ship.GamePlayer.Name.Equals(playerName))
            {
                return StateCode.InvalidPlayer;
            }

            ICollection<IGameShip> targetShips = GetVisibleTargetsForShip(playerName, ship, gameField, ActionType.Repair);

            if (targetShips.Count == 0)
            {
                return StateCode.OutOfDistance;
            }

            foreach (var tShip in targetShips)
            {
                tShip.Hp += Convert.ToUInt16(ship.RepairPower / targetShips.Count);
                if (tShip.Hp > tShip.MaxHp)
                {
                    tShip.Hp = tShip.MaxHp;
                }
            }

            await _gameFieldRepository.UpdateAsync(gameField);

            return StateCode.Success;
        }

        #endregion

        public async Task<StateCode> NextMove(int gameId)
        {
            var query = await _gameRepository.GetWithIncludeAsync(g => g.Id == gameId, g => g.GamePlayers);

            Game game = query.FirstOrDefault();

            ICollection<GamePlayer> players = game.GamePlayers;

            for (int i = 0; i < players.Count; i++)
            {
                if (players.ElementAt(i).Id == game.CurrentGamePlayerMoveId)
                {
                    game.CurrentGamePlayerMoveId =
                        i + 1 < players.Count ? players.ElementAt(i + 1).Id : players.ElementAt(0).Id;

                    return StateCode.Success;
                }
            }

            return StateCode.InvalidPlayer;
        }

        /// <summary>
        /// Check end game after action and change state <see cref="IGame"/>
        /// </summary>
        /// <returns>true if game finished, otherwise false</returns>
        protected bool CheckEndGame(Game game)
        {
            IGamePlayer player = null;
            for (ushort i = 1; i <= game.GameField.SizeX; i++)
            {
                for (ushort j = 1; j < game.GameField.SizeY; j++)
                {
                    if (game.GameField[i, j] != null)
                    {
                        if (player != null && game.GameField[i, j].GamePlayer != player)
                        {
                            return false;
                        }
                        else
                        {
                            player = game.GameField[i, j].GamePlayer;
                        }
                    }
                }
            }

            game.GameState = GameState.Finished;
            game.WinnerId = player.Id;

            return true;
        }
    }
}