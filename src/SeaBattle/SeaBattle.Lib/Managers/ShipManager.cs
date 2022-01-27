using SeaBattle.Lib.Entities;
using System.Collections.Generic;
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
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="ICommonShip"/></value>
        protected readonly ICollection<ICommonShip> Ships;

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
            Ships = new List<ICommonShip>();

            Ships.Add(new Ship(++_entityCount, ShipType.Auxiliary, 1, 100, 4));
            Ships.Add(new Ship(++_entityCount, ShipType.Mixed, 2, 200, 3));
            Ships.Add(new Ship(++_entityCount, ShipType.Mixed, 3, 300, 2));
            Ships.Add(new Ship(++_entityCount, ShipType.Military, 4, 400, 1));

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

        public ICollection<ICommonShip> GetShips() => Ships;

        public ICollection<IRepair> GetRepairs() => Repairs;

        public ICollection<IWeapon> GetWeapons() => Weapons;

        public IGameShip GetNewShip(IGamePlayer gamePlayer, ICommonShip ship) =>
            new GameShip(++_entityCount, ship, gamePlayer, GetShipCost(ship.Size));

        public StateCode AddWeapon(IGamePlayer gamePlayer, IGameShip gameShip, IWeapon weapon)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
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

            gameShip.Repairs.Add(repair);

            return StateCode.Success;
        }

        public StateCode RemoveWeapon(IGamePlayer gamePlayer, IGameShip gameShip, IWeapon weapon)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            gameShip.Weapons.Remove(weapon);

            return StateCode.Success;
        }

        public StateCode RemoveRepair(IGamePlayer gamePlayer, IGameShip gameShip, IRepair repair)
        {
            if (gameShip.GamePlayer != gamePlayer)
            {
                return StateCode.InvalidPlayer;
            }

            gameShip.Repairs.Remove(repair);

            return StateCode.Success;
        }

        /// <summary>
        /// Calculation ship cost by his size.
        /// </summary>
        /// <param name="size">Size (length) of ship.</param>
        /// <returns>Amount of points of cost ship.</returns>
        protected int GetShipCost(byte size) => size * PriceCoefficient;
    }
}
