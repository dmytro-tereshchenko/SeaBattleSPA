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
        public uint GameId { get; set; }

        /// <summary>
        /// Link to game field
        /// </summary>
        /// <value><see cref="IGameField"/></value>
        IGameField GameField { get; set; }

        /// <summary>
        /// Array labels for game field when the player can put his own ships on start field.
        /// </summary>
        /// <value>Array <see cref="bool"/>[,], when element = true - can put ship ; false - can't</value>
        bool[,] FieldLabels { get; set; }

        /// <summary>
        /// The player owning this starting
        /// </summary>
        /// <value><see cref="IGamePlayer"/></value>
        public IGamePlayer GamePlayer { get; set; }

        /// <summary>
        /// Points for buying ships
        /// </summary>
        /// <value><see cref="int"/></value>
        int Points { get; set; }

        /// <summary>
        /// Collection of ships that bought but don't put to the field
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"></see></value>
        ICollection<IGameShip> Ships { get; set; }
    }
}
