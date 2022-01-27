using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Responses
{
    /// <summary>
    /// Response with <see cref="IStartField"/>
    /// </summary>
    public interface IResponseStartField : IResponse
    {
        /// <summary>
        /// Start field with the game field, ships, and zone for placing ships.
        /// </summary>
        /// <value><see cref="IStartField"/></value>
        IStartField Value { get; }
    }
}
