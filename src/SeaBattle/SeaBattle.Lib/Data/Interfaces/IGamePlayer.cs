using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player (user) in the game
    /// </summary>
    public interface IGamePlayer : IPlayer
    {
        /// <summary>
        /// Player's state
        /// </summary>
        /// <value><see cref="PlayerState"/></value>
        public PlayerState State { get; set; }
    }
}
