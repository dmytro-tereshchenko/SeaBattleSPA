using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player's state in game
    /// </summary>
    public interface IPlayerState : IEntity
    {
        /// <summary>
        /// Name of the player state.
        /// </summary>
        /// <value><see cref="string"/></value>
        string Name { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="GamePlayer"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="GamePlayer"/>.</value>
        ICollection<GamePlayer> GamePlayers { get; set; }
    }
}
