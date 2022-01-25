using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Response with game field
    /// </summary>
    public interface IResponseGameField : IResponse
    {
        /// <summary>
        /// Game field of game
        /// </summary>
        /// <value><see cref="IGameField"/></value>
        public IGameField Value { get; }
    }
}
