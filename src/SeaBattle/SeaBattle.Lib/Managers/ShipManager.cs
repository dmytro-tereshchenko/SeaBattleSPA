using SeaBattle.Lib.Entities;
using System.Collections.Generic;
using System.Linq;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for creating and change ships, implements <see cref="IShipManager"/>.
    /// </summary>
    public class ShipManager : IShipManager
    {
        /// <summary>
        /// Price for 1 cell of ship
        /// </summary>
        /// <value><see cref="int"/></value>
        protected const int PriceCoefficient = 1000;

        /// <summary>
        /// Collection of basic types of common ships
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is (<see cref="ICommonShip"/>, <see cref="int"/>) (ship, points cost)</value>
        protected readonly ICollection<(ICommonShip, int)> Ships;

        /// <summary>
        /// Collection of basic weapons
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IWeapon"/></value>
        protected readonly ICollection<IWeapon> Weapons;

        /// <summary>
        /// Collection of basic repairs
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IRepair"/></value>
        protected readonly ICollection<IRepair> Repairs;

        /// <summary>
        /// Count of created entities.
        /// </summary>
        private uint _entityCount;

        public ShipManager()
        {
            Ships = new List<(ICommonShip, int)>();

            Ships.Add(new(new CommonShip(++_entityCount, ShipType.Auxiliary, 1, 100, 4), GetShipCost(1)));
            Ships.Add(new(new CommonShip(++_entityCount, ShipType.Mixed, 2, 200, 3), GetShipCost(2)));
            Ships.Add(new(new CommonShip(++_entityCount, ShipType.Mixed, 3, 300, 2), GetShipCost(3)));
            Ships.Add(new(new CommonShip(++_entityCount, ShipType.Military, 4, 400, 1), GetShipCost(4)));

            Repairs = new List<IRepair>();

            Repairs.Add(new BasicRepair(++_entityCount, 40, 10));

            Weapons = new List<IWeapon>();

            Weapons.Add(new BasicWeapon(++_entityCount, 50, 10));
        }

        public StateCode BuyShip(ICollection<IGamePlayer> players, IGameShip gameShip, IStartField startField)
        {
            if (!players.Contains(startField.GamePlayer))
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

        public StateCode SellShip(ICollection<IGamePlayer> players, IGameShip gameShip, IStartField startField)
        {
            if (!players.Contains(startField.GamePlayer))
            {
                return StateCode.InvalidPlayer;
            }

            startField.Ships.Remove(gameShip);
            startField.Points += gameShip.Points;

            return StateCode.Success;
        }

        public ICollection<(ICommonShip, int)> GetShips() => Ships;

        public ICollection<IRepair> GetRepairs() => Repairs;

        public ICollection<IWeapon> GetWeapons() => Weapons;

        public IGameShip GetNewShip(IGamePlayer gamePlayer, ICommonShip ship)
        {
            IGameShip gameShip = new GameShip(++_entityCount, ship, gamePlayer, GetShipCost(ship.Size));
            switch (ship.Type)
            {
                case ShipType.Military:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        AddWeapon(gamePlayer, gameShip, Weapons.First());
                    }
                    break;
                case ShipType.Auxiliary:
                    for (int i = 0; i < ship.Size; i++)
                    {
                        AddRepair(gamePlayer, gameShip, Repairs.First());
                    }
                    break;
            }

            return gameShip;
        }

        public StateCode AddWeapon(IGamePlayer gamePlayer, IGameShip gameShip, IWeapon weapon)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if (gameShip.Ship.Type == ShipType.Auxiliary)
            {
                return StateCode.InvalidEquipment;
            }

            gameShip.Weapons.Add(weapon);

            return StateCode.Success;
        }

        public StateCode AddRepair(IGamePlayer gamePlayer, IGameShip gameShip, IRepair repair)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            if (gameShip.Weapons.Count + gameShip.Repairs.Count == gameShip.Size)
            {
                return StateCode.LimitEquipment;
            }

            if (gameShip.Ship.Type == ShipType.Military)
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

            return gameShip.Weapons.Remove(weapon) ? StateCode.Success : StateCode.InvalidEquipment;
        }

        public StateCode RemoveRepair(IGamePlayer gamePlayer, IGameShip gameShip, IRepair repair)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            return gameShip.Repairs.Remove(repair) ? StateCode.Success : StateCode.InvalidEquipment;
        }

        /// <summary>
        /// Calculation ship cost by his size.
        /// </summary>
        /// <param name="size">Size (length) of ship.</param>
        /// <returns>Amount of points of cost ship.</returns>
        protected int GetShipCost(byte size) => size * PriceCoefficient;
    }
}
