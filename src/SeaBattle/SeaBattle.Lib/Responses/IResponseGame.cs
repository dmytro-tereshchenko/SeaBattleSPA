using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Responses
{
    /// <summary>
    /// Response with <see cref="IGamePlayer"/>
    /// </summary>
    public interface IResponseGame : IResponse
    {
        /// <summary>
        /// Game player which used in game.
        /// </summary>
        /// <value><see cref="IGame"/></value>
        IGame Value { get; }
    }
}
