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
        private const int PriceCoefficient = 1000;

        /// <summary>
        /// Buy ship and add to <see cref="IStartField"/>
        /// </summary>
        /// <param name="players">Collection of players in the game</param>
        /// <param name="gameShip">Game ship</param>
        /// <param name="startField">Start field with initializing data and parameters for the player</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
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

        /// <summary>
        /// Sell ship and remove from <see cref="IStartField"/>
        /// </summary>
        /// <param name="players">Collection of players in the game</param>
        /// <param name="gameShip">Game ship</param>
        /// <param name="startField">Start field with initializing data and parameters for the player</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
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

        /// <summary>
        /// Getting a collection of ships, which players can buy by points.
        /// </summary>
        /// <returns>Collection of <see cref="ICommonShip"/>, which players can buy by points</returns>
        public ICollection<ICommonShip> GetShips()
        {
            ICollection<ICommonShip> ships = new List<ICommonShip>();

            ships.Add(new Ship(ShipType.Auxiliary, 1, 100, 4));
            ships.Add(new Ship(ShipType.Mixed, 2, 200, 3));
            ships.Add(new Ship(ShipType.Mixed, 3, 300, 2));
            ships.Add(new Ship(ShipType.Military, 4, 400, 1));

            return ships;
        }

        /// <summary>
        /// Getting a collection of repairs, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IRepair"/>, which players can equip on the ship</returns>
        public ICollection<IRepair> GetRepairs()
        {
            ICollection<IRepair> repairs = new List<IRepair>();

            repairs.Add(new BasicRepair(40, 10));

            return repairs;
        }

        /// <summary>
        /// Getting a collection of weapons, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IWeapon"/>, which players can equip on the ship</returns>
        public ICollection<IWeapon> GetWeapons()
        {
            ICollection<IWeapon> weapons = new List<IWeapon>();

            weapons.Add(new BasicWeapon(50, 10));

            return weapons;
        }

        /// <summary>
        /// Creation and getting a new game ship.
        /// </summary>
        /// <param name="player">Player(user) in game</param>
        /// <param name="ship">Type of <see cref="ICommonShip"/>, which player wants to buy.</param>
        /// <returns><see cref="IGameShip"/> Game ship.</returns>
        public IGameShip GetNewShip(IPlayer player, ICommonShip ship) =>
            new GameShip(ship, player, GetShipCost(ship.Size));

        /// <summary>
        /// Add weapon to game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which adds a weapon.</param>
        /// <param name="weapon">A weapon (<see cref="IWeapon"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
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

        /// <summary>
        /// Add repair to game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which adds a repair.</param>
        /// <param name="repair">A repair (<see cref="IRepair"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
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

        /// <summary>
        /// Remove weapon from game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which removes a weapon.</param>
        /// <param name="weapon">Weapon (<see cref="IWeapon"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
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

        /// <summary>
        /// Remove repair from game ship.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="gameShip">Game ship which removes a repair.</param>
        /// <param name="repair">Repair (<see cref="IRepair"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
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
