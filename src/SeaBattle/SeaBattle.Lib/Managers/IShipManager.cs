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
        /// <param name="gameShipId">Game ship's Id</param>
        /// <param name="startFieldId">Start field's Id with initializing data and parameters for the player</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> BuyShip(int gameShipId, int startFieldId);

        /// <summary>
        /// Sell ship and remove from <see cref="StartField"/>
        /// </summary>
        /// <param name="gameShipId">Game ship's Id</param>
        /// <param name="startFieldId">Start field's Id with initializing data and parameters for the player</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> SellShip(int gameShipId, int startFieldId);

        /// <summary>
        /// Creation and getting a new game ship.
        /// </summary>
        /// <param name="gamePlayerId">Id of Player(user) in game</param>
        /// <param name="shipId">Id of Type of <see cref="Ship"/>, which player wants to buy.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="IGameShip"/> or <see cref="null"/> in case of error</returns>
        Task<IGameShip> GetNewShip(int gamePlayerId, int shipId);

        /// <summary>
        /// Add weapon to game ship.
        /// </summary>
        /// <param name="gameShipId">Game ship's Id which adds a weapon.</param>
        /// <param name="weaponId">A weapon's Id (<see cref="Weapon"/>) which adds.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> AddWeapon(int gameShipId, int weaponId);

        /// <summary>
        /// Add repair to game ship.
        /// </summary>
        /// <param name="gameShipId">Game ship's Id which adds a repair.</param>
        /// <param name="repairId">Id of repair (<see cref="Repair"/>) which adds.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> AddRepair(int gameShipId, int repairId);

        /// <summary>
        /// Remove weapon from game ship.
        /// </summary>
        /// <param name="gameShipId">Game ship's Id which removes a weapon.</param>
        /// <param name="weaponId">Weapon's Id (<see cref="Weapon"/>) which removes.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> RemoveWeapon(int gameShipId, int weaponId);

        /// <summary>
        /// Remove repair from game ship.
        /// </summary>
        /// <param name="gameShipId">Game ship's Id which removes a repair.</param>
        /// <param name="repairId">Id of Repair (<see cref="Repair"/>) which removes.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains <see cref="StateCode"/></returns>
        Task<StateCode> RemoveRepair(int gameShipId, int repairId);
    }
}
