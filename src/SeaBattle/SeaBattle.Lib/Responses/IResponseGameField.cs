using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Responses
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
        IGameField Value { get; }
    }
}
