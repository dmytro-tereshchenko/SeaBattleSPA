using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Responses
{
    public class ResponseGamePlayer : IResponseGamePlayer
    {
        public StateCode State { get; private set; }

        public IGamePlayer Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseGamePlayer"/> class
        /// </summary>
        /// <param name="value">Game player</param>
        /// <param name="state">State of response</param>
        public ResponseGamePlayer(IGamePlayer value, StateCode state)
        {
            Value = value;
            State = state;
        }
    }
}
