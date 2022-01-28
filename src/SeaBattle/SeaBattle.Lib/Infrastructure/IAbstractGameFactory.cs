using SeaBattle.Lib.Managers;

namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Abstract factory to initialize <see cref="IGameManager"/>
    /// </summary>
    public interface IAbstractGameFactory
    {
        /// <summary>
        /// Get initialize manager for <see cref="IGameManager"/>
        /// </summary>
        /// <returns><see cref="IInitializeManager"/></returns>
        IInitializeManager GetInitializeManager();

        /// <summary>
        /// Get ship manager for <see cref="IGameManager"/>
        /// </summary>
        /// <returns><see cref="IShipManager"/></returns>
        IShipManager GetShipManager();

        /// <summary>
        /// Get action manager for <see cref="IGameManager"/>
        /// </summary>
        /// <returns><see cref="IActionManager"/></returns>
        IActionManager GetActionManager();
    }
}
