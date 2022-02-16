namespace SeaBattle.Lib.Managers
{
    /// <summary>
    /// Manager which response for creating and change ships.
    /// </summary>
    public interface IShipManager
    {
        /// <summary>
        /// Buy ship and add to <see cref="IStartField"/>
        /// </summary>
        /// <param name="players">Collection of players in the game</param>
        /// <param name="gameShip">Game ship</param>
        /// <param name="startField">Start field with initializing data and parameters for the player</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        /*StateCode BuyShip(ICollection<IGamePlayer> players, IGameShip gameShip, IStartField startField);

        /// <summary>
        /// Sell ship and remove from <see cref="IStartField"/>
        /// </summary>
        /// <param name="players">Collection of players in the game</param>
        /// <param name="gameShip">Game ship</param>
        /// <param name="startField">Start field with initializing data and parameters for the player</param>
        /// <returns><see cref="StateCode"/> result of operation</returns>
        StateCode SellShip(ICollection<IGamePlayer> players, IGameShip gameShip, IStartField startField);

        /// <summary>
        /// Getting a collection of ships, which players can buy by points.
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is (<see cref="IShip"/>, <see cref="int"/>) (ship, points cost)</value>
        ICollection<(IShip, int)> GetShips();

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
        /// <param name="gamePlayer">Player(user) in game</param>
        /// <param name="ship">Type of <see cref="IShip"/>, which player wants to buy.</param>
        /// <returns><see cref="IGameShip"/> Game ship.</returns>
        IGameShip GetNewShip(IGamePlayer gamePlayer, IShip ship);

        /// <summary>
        /// Add weapon to game ship.
        /// </summary>
        /// <param name="gamePlayer">Current player</param>
        /// <param name="gameShip">Game ship which adds a weapon.</param>
        /// <param name="weapon">A weapon (<see cref="IWeapon"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode AddWeapon(IGamePlayer gamePlayer, IGameShip gameShip, IWeapon weapon);

        /// <summary>
        /// Add repair to game ship.
        /// </summary>
        /// <param name="gamePlayer">Current player</param>
        /// <param name="gameShip">Game ship which adds a repair.</param>
        /// <param name="repair">A repair (<see cref="IRepair"/>) which adds.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode AddRepair(IGamePlayer gamePlayer, IGameShip gameShip, IRepair repair);

        /// <summary>
        /// Remove weapon from game ship.
        /// </summary>
        /// <param name="gamePlayer">Current player</param>
        /// <param name="gameShip">Game ship which removes a weapon.</param>
        /// <param name="weapon">Weapon (<see cref="IWeapon"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode RemoveWeapon(IGamePlayer gamePlayer, IGameShip gameShip, IWeapon weapon);

        /// <summary>
        /// Remove repair from game ship.
        /// </summary>
        /// <param name="gamePlayer">Current player</param>
        /// <param name="gameShip">Game ship which removes a repair.</param>
        /// <param name="repair">Repair (<see cref="IRepair"/>) which removes.</param>
        /// <returns><see cref="StateCode"/> is result of operation</returns>
        StateCode RemoveRepair(IGamePlayer gamePlayer, IGameShip gameShip, IRepair repair);*/
    }
}
