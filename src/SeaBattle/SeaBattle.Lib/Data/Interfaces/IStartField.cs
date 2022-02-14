using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// The field for storing the location of ships and points for buy ships by the player when initializing game.
    /// </summary>
    public interface IStartField : IEntity
    {
        /// <summary>
        /// Id of game <see cref="IGame"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GameId { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="IGameField"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GameFieldId { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="IGamePlayer"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GamePlayerId { get; set; }

        /// <summary>
        /// Collection of labels for game field when the player can put his own ships on start field
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IStartFieldCell"/></value>
        ICollection<IStartFieldCell> StartFieldCells { get; set; }

        /// <summary>
        /// Points for buying ships
        /// </summary>
        /// <value><see cref="int"/></value>
        int Points { get; set; }

        /// <summary>
        /// Navigate property <see cref="IGameField"/>
        /// </summary>
        /// <value><see cref="IGameField"/></value>
        IGameField GameField { get; set; }

        /// <summary>
        /// Navigate property <see cref="IGamePlayer"/>
        /// </summary>
        /// <value><see cref="IGamePlayer"/></value>
        IGamePlayer GamePlayer { get; set; }

        /// <summary>
        /// Collection of ships that bought but don't put to the field
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"></see></value>
        ICollection<IGameShip> GameShips { get; set; }

        /// <summary>
        /// Navigate property <see cref="IGame/>
        /// </summary>
        /// <value><see cref="IGame"/></value>
        IGame Game { get; set; }
    }
}
