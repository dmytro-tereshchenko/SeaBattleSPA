using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for creating and initializing start entities.
    /// </summary>
    public interface IInitializeManager
    {
        /// <summary>
        /// Creating and getting game field.
        /// </summary>
        /// <param name="sizeX">Size X of created game field.</param>
        /// <param name="sizeY">Size Y of created game field.</param>
        /// <returns><see cref="IResponseGameField"/> where Value is <see cref="IGameField"/>, State is <see cref="StateCode"/></returns>
        Task<IResponseGameField> CreateGameFieldAsync(ushort sizeX, ushort sizeY);

        /// <summary>
        /// Method for calculation points which need to purchase ships.
        /// </summary>
        /// <param name="field">Field with placement ships on start game - array with type bool, where true - can placed ship, false - wrong cell.</param>
        /// <returns>Amount of points</returns>
        int CalculateStartPoints(bool[,] field);

        /// <summary>
        /// Buy ship and add to <see cref="IStartField"/>
        /// </summary>
        /// <param name="playerId">Id of team</param>
        /// <param name="gameShipId">Id of game ship</param>
        /// <param name="startFieldId">Id of start field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        Task<StateCode> BuyShipAsync(uint playerId, uint gameShipId, uint startFieldId);

        /// <summary>
        /// Sell ship and remove from <see cref="IStartField"/>
        /// </summary>
        /// <param name="playerId">Id of team</param>
        /// <param name="gameShipId">Id of game ship</param>
        /// <param name="startFieldId">Id of start field</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        Task<StateCode> SellShipAsync(uint playerId, uint gameShipId, uint startFieldId);

        /// <summary>
        /// Method for generating a collection of fields with labels for possible placing ships on start field for teams.
        /// </summary>
        /// <param name="gameFieldId">id of gaming field.</param>
        /// <param name="numberOfTeams">Amount of teams</param>
        /// <returns>Collection of fields with ships - arrays with type bool, where true - placed boat, false - empty</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="numberOfTeams"/> out of range</exception>
        /// <exception cref="ArgumentNullException">There is no gaming field for the given <paramref name="gameFieldId"/>.</exception>
        ICollection<bool[,]> GenerateStartFields(uint gameFieldId, byte numberOfTeams);

        /// <summary>
        /// Getting a collection of ships, which players can buy by points.
        /// </summary>
        /// <returns>Collection of <see cref="IShip"/>, which players can buy by points</returns>
        ICollection<IShip> GetShips();

        /// <summary>
        /// Getting a collection of repairs, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IRepair"/>, which players can equip on the ship</returns>
        ICollection<IRepair> GetRepairs();

        /// <summary>
        /// Getting a collection of weapons, which players can equip on the ship.
        /// </summary>
        /// <returns>Collection of <see cref="IWeapon"/>, which players can equip on the ship</returns>
        ICollection<IWeapon> GetWeapons();

        /// <summary>
        /// Creation and getting a new game ship.
        /// </summary>
        /// <param name="teamId">id of the team</param>
        /// <param name="ship">Type of <see cref="IShip"/>, which player wants to buy.</param>
        /// <returns><see cref="IGameShip"/> Game ship.</returns>
        Task<IGameShip> GetNewShipAsync(uint teamId, IShip ship);

        /// <summary>
        /// Add weapon to game ship.
        /// </summary>
        /// <param name="playerId">Id of team</param>
        /// <param name="gameShipId">Id of the game ship which adds a weapon.</param>
        /// <param name="weapon">A weapon (<see cref="IWeapon"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        Task<StateCode> AddWeaponAsync(uint playerId, uint gameShipId, IWeapon weapon);

        /// <summary>
        /// Add repair to game ship.
        /// </summary>
        /// <param name="teamId">Id of team</param>
        /// <param name="gameShipId">Id of the game ship which adds a repair.</param>
        /// <param name="repair">A repair (<see cref="IRepair"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        Task<StateCode> AddRepairAsync(uint teamId, uint gameShipId, IRepair repair);

        /// <summary>
        /// Remove weapon from game ship.
        /// </summary>
        /// <param name="playerId">Id of team</param>
        /// <param name="gameShipId">Id of the game ship which removes a weapon.</param>
        /// <param name="weaponId">Id of weapon (<see cref="IWeapon"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        Task<StateCode> RemoveWeaponAsync(uint playerId, uint gameShipId, uint weaponId);

        /// <summary>
        /// Remove repair from game ship.
        /// </summary>
        /// <param name="playerId">Id of team</param>
        /// <param name="gameShipId">Id of the game ship which removes a repair.</param>
        /// <param name="repairId">Id of repair (<see cref="IRepair"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        Task<StateCode> RemoveRepairAsync(uint playerId, uint gameShipId, uint repairId);

        /// <summary>
        /// Getting border size of the game field.
        /// </summary>
        /// <returns><see cref="ILimitSize"/></returns>
        ILimitSize GetLimitSizeField();

        /// <summary>
        /// Generating and getting names for team members of the game.
        /// </summary>
        /// <param name="numberOfTeams">Amount of team members</param>
        /// <returns><see cref="ICollection{T}" /> whose generic type argument is <see cref="string"/></returns>
        ICollection<string> GetTeamNames(byte numberOfTeams);

        /// <summary>
        /// Create <see cref="IGame"/> by numberOfTeams
        /// </summary>
        /// <param name="numberOfTeams">Number of team members in the game</param>
        /// <returns><see cref="uint"/> id of the created game</returns>
        /// <exception cref="ArgumentOutOfRangeException">A number of teams are out of possible values.</exception>
        Task<uint> CreateGameAsync(byte numberOfTeams);
    }
}
