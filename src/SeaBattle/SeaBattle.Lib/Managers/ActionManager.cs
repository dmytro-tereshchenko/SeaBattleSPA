using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// The manager responsible for the actions and changes of ships on the field during the game, implements <see cref="IActionManager"/>.
    /// </summary>
    public class ActionManager : IActionManager
    {
        public IGameFieldActionUtility ActionUtility { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionManager"/> class
        /// </summary>
        /// <param name="actionUtility">Utility for actions on <see cref="IGameField"/></param>
        public ActionManager(IGameFieldActionUtility actionUtility) => ActionUtility = actionUtility;
        
        #region Getting data from GameField
        public IResponseGameField GetGameField(IGamePlayer player, IGame game)
        {
            if (player == null || game == null || game.Field == null)
            {
                return new ResponseGameField(null, StateCode.NullReference);
            }

            if (!game.Players.Contains(player))
            {
                return new ResponseGameField(null, StateCode.InvalidPlayer);
            }

            return new ResponseGameField(game.Field, StateCode.Success);
        }

        public IDictionary<IGameShip, ICollection<(ushort, ushort)>> GetFieldWithShips(IGameField field, IGamePlayer player = null)
        {
            if (field == null)
            {
                throw new ArgumentNullException();
            }

            //Search coordinates of ships
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> ships =
                ActionUtility.GetAllShipsCoordinates(field, player);

            (float, float) centerField = (field.SizeX / 2, field.SizeY / 2);

            //sorting ships by distance from the center of the field
            return ships
                .OrderBy(s =>
                    ActionUtility.GetDistanceBetween2Points(centerField,
                        ActionUtility.GetGeometricCenterOfShip(s.Value)))
                .ToDictionary(s=>s.Key, s=>s.Value);
        }

        public ICollection<IGameShip> GetVisibleTargetsForShip(IGamePlayer player, IGameShip ship, IGameField field,
            ActionType action)
        {
            if (player == null || field == null || ship == null)
            {
                throw new NullReferenceException();
            }

            if (ship.GamePlayer != player)
            {
                throw new ArgumentException();
            }

            //Search coordinates of ships
            IDictionary<IGameShip, ICollection<(ushort, ushort)>> ships = ActionUtility.GetAllShipsCoordinates(field, player);

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
                ActionType.Attack => filteringShips.Where(s => s.GamePlayer != player),
                ActionType.Repair => filteringShips.Where(s => s.GamePlayer == player),
                _ => throw new InvalidEnumArgumentException()
            };

            return filteringShips.ToList();
        }
        #endregion

        #region Actions with StartField
        public StateCode RemoveShipFromFieldToStartField(IPlayer player, ushort tPosX, ushort tPosY,
            IStartField startField)
        {
            if (player == null || startField == null)
            {
                return StateCode.NullReference;
            }

            IGameShip ship = startField.GameField[tPosX, tPosY];

            if (ship == null)
            {
                return StateCode.AbsentOfShip;
            }

            if (ship.GamePlayer != player || startField.GamePlayer != player)
            {
                return StateCode.InvalidPlayer;
            }

            startField.Ships.Add(ship);
            startField.GameField[tPosX, tPosY] = null;

            return StateCode.Success;
        }

        public StateCode PutShipFromStartFieldToField(IGamePlayer player, ushort tPosX, ushort tPosY,
            DirectionOfShipPosition direction, IStartField startField, IGameShip ship)
        {
            if (player == null || startField == null || ship == null)
            {
                return StateCode.NullReference;
            }

            if (!startField.Ships.Contains(ship))
            {
                return StateCode.InvalidShip;
            }

            if (ship.GamePlayer != player || startField.GamePlayer != player)
            {
                return StateCode.InvalidPlayer;
            }

            StateCode result;
            try
            {
                result = ActionUtility.PutShipOnField(player, ship, tPosX, tPosY, direction, startField.GameField);
            }
            catch (InvalidEnumArgumentException)
            {
                result = StateCode.InvalidOperation;
            }

            if (result == StateCode.Success)
            {
                startField.Ships.Remove(ship);
            }

            return result;
        }
        #endregion

        #region Actions with ship
        public StateCode MoveShip(IGamePlayer player, IGameShip ship, ushort tPosX, ushort tPosY,
            DirectionOfShipPosition direction, IGameField field)
        {
            if (player == null || field == null || ship == null)
            {
                return StateCode.NullReference;
            }
            
            if (ship.GamePlayer != player)
            {
                return StateCode.InvalidPlayer;
            }

            //Ship's coordinates on game field
            List<(ushort, ushort)> locOfShip = new List<(ushort, ushort)>(ship.Size);

            for (ushort i = 1; i <= field.SizeX; i++)
            {
                for (ushort j = 1; j <= field.SizeY; j++)
                {
                    if (field[i, j] == ship)
                    {
                        locOfShip.Add((i,j));
                    }
                }
            }

            //check distance between target point and furthest cell of current ship.
            if (locOfShip.Select(s => ActionUtility.GetDistanceBetween2Points((tPosX, tPosY), s)).OrderByDescending(d => d).First() >
                ship.Speed)
            {
                return StateCode.OutOfDistance;
            }

            StateCode result;

            try
            {
                result = ActionUtility.PutShipOnField(player, ship, tPosX, tPosY, direction, field);
            }
            catch (InvalidEnumArgumentException)
            {
                result = StateCode.InvalidOperation;
            }

            if (result == StateCode.Success)
            {
                //clear previous position of ship
                foreach (var loc in locOfShip)
                {
                    field[loc.Item1, loc.Item2] = null;
                }
            }

            return result;
        }

        public StateCode AttackShip(IGamePlayer player, IGameShip ship, ushort tPosX, ushort tPosY, IGameField field)
        {
            if (player == null || field == null || ship == null)
            {
                return StateCode.NullReference;
            }

            if (ship.GamePlayer != player)
            {
                return StateCode.InvalidPlayer;
            }

            if (field[tPosX, tPosY] == null || field[tPosX, tPosY] == ship)
            {
                return StateCode.MissTarget;
            }

            (float, float) centerOfTargetPoint = ((float) tPosX - 0.5f, (float) tPosY - 0.5f);

            if (ActionUtility.GetDistanceBetween2Points(centerOfTargetPoint,
                    ActionUtility.GetGeometricCenterOfShip(ActionUtility.GetShipCoordinates(ship, field))) >
                ship.AttackRange)
            {
                return StateCode.OutOfDistance;
            }

            if (field[tPosX, tPosY].Hp < ship.Damage)
            {
                foreach (var shipsCell in ActionUtility.GetShipCoordinates(field[tPosX, tPosY], field))
                {
                    field[shipsCell.Item1, shipsCell.Item2] = null;
                }
                return StateCode.TargetDestroyed;
            }

            field[tPosX, tPosY].Hp -= ship.Damage;

            return StateCode.Success;
        }

        public StateCode RepairShip(IGamePlayer player, IGameShip ship, ushort tPosX, ushort tPosY, IGameField field)
        {
            if (player == null || field == null || ship == null)
            {
                return StateCode.NullReference;
            }

            if (ship.GamePlayer != player)
            {
                return StateCode.InvalidPlayer;
            }

            if (field[tPosX, tPosY] == null || field[tPosX, tPosY] == ship)
            {
                return StateCode.MissTarget;
            }

            (float, float) centerOfTargetPoint = ((float)tPosX - 0.5f, (float)tPosY - 0.5f);

            if (ActionUtility.GetDistanceBetween2Points(centerOfTargetPoint,
                    ActionUtility.GetGeometricCenterOfShip(ActionUtility.GetShipCoordinates(ship, field))) >
                ship.RepairRange)
            {
                return StateCode.OutOfDistance;
            }

            field[tPosX, tPosY].Hp += ship.RepairPower;
            if (field[tPosX, tPosY].Hp > field[tPosX, tPosY].MaxHp)
            {
                field[tPosX, tPosY].Hp = field[tPosX, tPosY].MaxHp;
            }

            return StateCode.Success;
        }

        public StateCode RepairAllShip(IGamePlayer player, IGameShip ship, IGameField field)
        {
            if (player == null || field == null || ship == null)
            {
                return StateCode.NullReference;
            }

            if (ship.GamePlayer != player)
            {
                return StateCode.InvalidPlayer;
            }

            ICollection<IGameShip> targetShips = GetVisibleTargetsForShip(player, ship, field, ActionType.Repair);

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

            return StateCode.Success;
        }
        #endregion
    }
}