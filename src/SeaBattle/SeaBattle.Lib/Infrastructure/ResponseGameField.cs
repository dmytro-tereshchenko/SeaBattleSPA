using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Response with game field
    /// </summary>
    public class ResponseGameField :IResponseGameField
    {
        /// <summary>
        /// Game field of game
        /// </summary>
        /// <value><see cref="IGameField"/></value>
        public IGameField Value { get; private set; }

        /// <summary>
        /// State of the response
        /// </summary>
        /// <value><see cref="StateCode"/></value>
        public StateCode State { get; private set; }

        public ResponseGameField(IGameField value, StateCode state)
        {
            Value = value;
            State = state;
        }
    }
}
