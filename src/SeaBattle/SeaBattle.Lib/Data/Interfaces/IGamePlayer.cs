using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player (user) in the game
    /// </summary>
    public interface IGamePlayer : IPlayer
    {
        /// <summary>
        /// Foreign key Id <see cref="PlayerState"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint PlayerStateId { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="Game"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="Game"/>.</value>
        ICollection<Game> Games { get; set; }

        /// <summary>
        /// Navigate property <see cref="PlayerState"/>
        /// </summary>
        /// <value><see cref="PlayerState"/></value>
        PlayerState PlayerState { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="GameShip"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="GameShip"/>.</value>
        ICollection<GameShip> GameShips { get; set; }

        /// <summary>
        /// Navigation property to collection <see cref="StartField"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="StartField"/>.</value>
        ICollection<StartField> StartFields { get; set; }
    }
}
