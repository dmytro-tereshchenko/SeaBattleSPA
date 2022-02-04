using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Responses
{
    /// <summary>
    /// Response with <see cref="IGamePlayer"/>
    /// </summary>
    public interface IResponseGamePlayer : IResponse
    {
        /// <summary>
        /// Game player which used in game.
        /// </summary>
        /// <value><see cref="IGamePlayer"/></value>
        IGamePlayer Value { get; }
    }
}
