using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Managers;

namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Abstract factory to initialize <see cref="IGameManager"/>
    /// </summary>
    public class AbstractGameFactory //: IAbstractGameFactory
    {
        /// <summary>
        /// Max number of players in <see cref="IGame"/>
        /// </summary>
        private byte _maxNumberOfPlayers;

        /// <summary>
        /// Max size X of <see cref="IGameField"/>
        /// </summary>
        private ushort _maxSizeX;

        /// <summary>
        /// Min size X of <see cref="IGameField"/>
        /// </summary>
        private ushort _minSizeX;

        /// <summary>
        /// Max size Y of <see cref="IGameField"/>
        /// </summary>
        private ushort _maxSizeY;

        /// <summary>
        /// Min size Y of <see cref="IGameField"/>
        /// </summary>
        private ushort _minSizeY;

        /// <summary>
        /// Action utility for working with <see cref="IGameField"/>
        /// </summary>
        private IGameFieldActionUtility _actionUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGameFactory"/> class
        /// </summary>
        /// <param name="maxNumberOfPlayers">Max number of players in <see cref="IGame"/></param>
        /// <param name="maxSizeX">Max size X of <see cref="IGameField"/></param>
        /// <param name="minSizeX">Min size X of <see cref="IGameField"/></param>
        /// <param name="maxSizeY">Max size Y of <see cref="IGameField"/></param>
        /// <param name="minSizeY">Min size Y of <see cref="IGameField"/></param>
        /// <param name="actionUtility">Action utility for working with <see cref="IGameField"/></param>
        public AbstractGameFactory(byte maxNumberOfPlayers, ushort maxSizeX, ushort minSizeX, ushort maxSizeY, ushort minSizeY, IGameFieldActionUtility actionUtility)
        {
            _maxNumberOfPlayers = maxNumberOfPlayers;
            _maxSizeX = maxSizeX;
            _minSizeX = minSizeX;
            _maxSizeY = maxSizeY;
            _minSizeY = minSizeY;
            _actionUtility = actionUtility;
        }

        public IInitializeManager GetInitializeManager() =>
            new InitializeManager(_minSizeX, _maxSizeX, _minSizeY, _maxSizeY, _maxNumberOfPlayers);

        //public IShipManager GetShipManager() => new ShipManager();

        public IActionManager GetActionManager() => new ActionManager(_actionUtility);
    }
}
