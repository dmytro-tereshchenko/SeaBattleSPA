using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player (user) in the game
    /// </summary>
    public interface IGamePlayer : IPlayer
    {
        /// <summary>
        /// Foreign key Id <see cref="IPlayerState"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint PlayerStateId { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="IGame"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGame"/>.</value>
        ICollection<IGame> Games { get; set; }

        /// <summary>
        /// Navigate property <see cref="IPlayerState"/>
        /// </summary>
        /// <value><see cref="IPlayerState"/></value>
        IPlayerState PlayerState { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="IGameShip"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"/>.</value>
        ICollection<IGameShip> GameShips { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="IStartField"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IStartField"/>.</value>
        ICollection<IStartField> StartFields { get; set; }
    }
}
