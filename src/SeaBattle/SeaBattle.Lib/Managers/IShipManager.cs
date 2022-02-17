using System.Threading.Tasks;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for creating and change ships.
    /// </summary>
    public interface IShipManager
    {
        /// <summary>
        /// Buy ship and add to <see cref="StartField"/>
        /// </summary>
        /// <param name="gameShip">Game ship</param>
        /// <param name="startField">Start field with initializing data and parameters for the player</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> BuyShip(GameShip gameShip, StartField startField);

        /// <summary>
        /// Sell ship and remove from <see cref="StartField"/>
        /// </summary>
        /// <param name="gameShip">Game ship</param>
        /// <param name="startField">Start field with initializing data and parameters for the player</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> SellShip(GameShip gameShip, StartField startField);

        /// <summary>
        /// Creation and getting a new game ship.
        /// </summary>
        /// <param name="gamePlayer">Player(user) in game</param>
        /// <param name="ship">Type of <see cref="Ship"/>, which player wants to buy.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="GameShip"/> or <see cref="null"/> in case of error</returns>
        Task<GameShip> GetNewShip(GamePlayer gamePlayer, Ship ship);

        /// <summary>
        /// Add weapon to game ship.
        /// </summary>
        /// <param name="gameShip">Game ship which adds a weapon.</param>
        /// <param name="weapon">A weapon (<see cref="Weapon"/>) which adds.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> AddWeapon(GameShip gameShip, Weapon weapon);

        /// <summary>
        /// Add repair to game ship.
        /// </summary>
        /// <param name="gameShip">Game ship which adds a repair.</param>
        /// <param name="repair">A repair (<see cref="Repair"/>) which adds.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> AddRepair(GameShip gameShip, Repair repair);

        /// <summary>
        /// Remove weapon from game ship.
        /// </summary>
        /// <param name="gameShip">Game ship which removes a weapon.</param>
        /// <param name="weapon">Weapon (<see cref="Weapon"/>) which removes.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> RemoveWeapon(GameShip gameShip, Weapon weapon);

        /// <summary>
        /// Remove repair from game ship.
        /// </summary>
        /// <param name="gameShip">Game ship which removes a repair.</param>
        /// <param name="repair">Repair (<see cref="Repair"/>) which removes.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> RemoveRepair(GameShip gameShip, Repair repair);
    }
}
