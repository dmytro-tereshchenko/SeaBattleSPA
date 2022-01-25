using SeaBattle.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private const int PriceCoefficient = 1000;

        public StateCode BuyShip(ICollection<IPlayer> players, IGameShip gameShip, IStartField startField)
        {
            if (!players.Contains(startField.Player))
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

        public StateCode SellShip(ICollection<IPlayer> players, IGameShip gameShip, IStartField startField)
        {
            if (!players.Contains(startField.Player))
            {
                return StateCode.InvalidPlayer;
            }

            startField.Ships.Remove(gameShip);
            startField.Points += gameShip.Points;

            return StateCode.Success;
        }

        public ICollection<ICommonShip> GetShips()
        {
            ICollection<ICommonShip> ships = new List<ICommonShip>();

            ships.Add(new Ship(ShipType.Auxiliary, 1, 100, 4));
            ships.Add(new Ship(ShipType.Mixed, 2, 200, 3));
            ships.Add(new Ship(ShipType.Mixed, 3, 300, 2));
            ships.Add(new Ship(ShipType.Military, 4, 400, 1));

            return ships;
        }

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

        public IGameShip GetNewShip(IPlayer player, ICommonShip ship) =>
            new GameShip(ship, player, GetShipCost(ship.Size));

        public StateCode AddWeapon(IPlayer player, IGameShip gameShip, IWeapon weapon)
        {
            if (gameShip == null || weapon == null || player == null)
            {
                return StateCode.NullReference;
            }

            if (gameShip.Player != player)
            {
                return StateCode.InvalidPlayer;
            }

            gameShip.Ship.Weapons.Add(weapon);

            return StateCode.Success;
        }

        public StateCode AddRepair(IPlayer player, IGameShip gameShip, IRepair repair)
        {
            if (gameShip == null || player == null || repair == null)
            {
                return StateCode.NullReference;
            }

            if (gameShip.Player != player)
            {
                return StateCode.InvalidPlayer;
            }

            gameShip.Ship.Repairs.Add(repair);

            return StateCode.Success;
        }

        public StateCode RemoveWeapon(IPlayer player, IGameShip gameShip, IWeapon weapon)
        {
            if (gameShip == null || weapon == null || player == null)
            {
                return StateCode.NullReference;
            }

            if (gameShip.Player != player)
            {
                return StateCode.InvalidPlayer;
            }

            gameShip.Ship.Weapons.Remove(weapon);

            return StateCode.Success;
        }

        public StateCode RemoveRepair(IPlayer player, IGameShip gameShip, IRepair repair)
        {

            if (gameShip == null || repair == null || player == null)
            {
                return StateCode.NullReference;
            }

            if (gameShip.Player != player)
            {
                return StateCode.InvalidPlayer;
            }

            gameShip.Ship.Repairs.Remove(repair);

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
