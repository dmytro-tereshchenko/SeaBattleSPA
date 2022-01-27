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

        public ShipManager()
        {
            Ships = new List<ICommonShip>();

            Ships.Add(new Ship(ShipType.Auxiliary, 1, 100, 4));
            Ships.Add(new Ship(ShipType.Mixed, 2, 200, 3));
            Ships.Add(new Ship(ShipType.Mixed, 3, 300, 2));
            Ships.Add(new Ship(ShipType.Military, 4, 400, 1));
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

            public ICollection<IRepair> GetRepairs()
        {
            ICollection<IRepair> repairs = new List<IRepair>();

            repairs.Add(new BasicRepair(40, 10));

            return repairs;
        }

        public ICollection<IWeapon> GetWeapons()
        {
            ICollection<IWeapon> weapons = new List<IWeapon>();

            weapons.Add(new BasicWeapon(50, 10));

            return weapons;
        }

        public IGameShip GetNewShip(IGamePlayer gamePlayer, ICommonShip ship) =>
            new GameShip(ship, gamePlayer, GetShipCost(ship.Size));

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
