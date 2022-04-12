using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Responses
{
    public class ResponseGame : IResponseGame
    {
        public StateCode State { get; private set; }

        public IGame Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseGame"/> class
        /// </summary>
        /// <param name="value">Game</param>
        /// <param name="state">State of response</param>
        public ResponseGame(IGame value, StateCode state)
        {
            Value = value;
            State = state;
        }
    }
}
