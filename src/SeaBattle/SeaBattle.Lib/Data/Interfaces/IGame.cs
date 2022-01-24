using System.Collections.Generic;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Game (match) between players.
    /// </summary>
    public interface IGame: IEntity
    {
        /// <summary>
        /// Id of game field <see cref="IGameField"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint FieldId { get; set; }

        /// <summary>
        /// Current amount of players which connected to the game
        /// </summary>
        /// <value><see cref="byte"/></value>
        public byte CurrentCountPlayers { get; set; }

        /// <summary>
        /// Max amount of players
        /// </summary>
        /// <value><see cref="byte"/></value>
        public byte MaxNumberOfPlayers { get; set; }

        /// <summary>
        /// Name of player, which needs to move this turn.
        /// </summary>
        /// <value><see cref="string"/> Team's name</value>
        string CurrentPlayerMove { get; set; }

        /// <summary>
        /// Current state of game
        /// </summary>
        /// <value><see cref="StateGame"/></value>
        StateGame State { get; set; }

        /// <summary>
        /// Collection of players id for current game.
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="uint"/></value>
        ICollection<uint> PlayersId { get; set; }
    }
}
