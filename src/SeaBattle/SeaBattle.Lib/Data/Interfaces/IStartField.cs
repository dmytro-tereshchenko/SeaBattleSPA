using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// The field for storing the location of ships and points for buy ships by the player when initializing game.
    /// </summary>
    public interface IStartField : IEntity
    {
        /// <summary>
        /// Id of game <see cref="Game"/>
        /// </summary>
        /// <value><see cref="int"/></value>
        int GameId { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="GameField"/>
        /// </summary>
        /// <value><see cref="int"/></value>
        int GameFieldId { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="GamePlayer"/>
        /// </summary>
        /// <value><see cref="int"/></value>
        int GamePlayerId { get; set; }

        /// <summary>
        /// Collection of labels for game field when the player can put his own ships on start field
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="StartFieldCell"/></value>
        ICollection<StartFieldCell> StartFieldCells { get; set; }

        /// <summary>
        /// Points for buying ships
        /// </summary>
        /// <value><see cref="int"/></value>
        int Points { get; set; }

        /// <summary>
        /// Navigate property <see cref="GameField"/>
        /// </summary>
        /// <value><see cref="GameField"/></value>
        GameField GameField { get; set; }

        /// <summary>
        /// Navigate property <see cref="GamePlayer"/>
        /// </summary>
        /// <value><see cref="GamePlayer"/></value>
        GamePlayer GamePlayer { get; set; }

        /// <summary>
        /// Collection of ships that bought but don't put to the field
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="GameShip"></see></value>
        ICollection<GameShip> GameShips { get; set; }

        /// <summary>
        /// Navigate property <see cref="Game"/>
        /// </summary>
        /// <value><see cref="Game"/></value>
        Game Game { get; set; }
    }
}
