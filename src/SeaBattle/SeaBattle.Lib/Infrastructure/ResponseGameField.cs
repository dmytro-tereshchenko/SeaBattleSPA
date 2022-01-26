using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Response with game field
    /// </summary>
    public class ResponseGameField :IResponseGameField
    {
        public IGameField Value { get; private set; }

        public StateCode State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseGameField"/> class
        /// </summary>
        /// <param name="value">Game field</param>
        /// <param name="state">State of response</param>
        public ResponseGameField(IGameField value, StateCode state)
        {
            Value = value;
            State = state;
        }
    }
}
