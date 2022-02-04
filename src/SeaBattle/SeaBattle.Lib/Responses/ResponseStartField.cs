using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Responses
{
    /// <summary>
    /// Response with <see cref="IStartField"/>
    /// </summary>
    public class ResponseStartField : IResponseStartField
    {
        public IStartField Value { get; private set; }

        public StateCode State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseStartField"/> class
        /// </summary>
        /// <param name="value">Start field</param>
        /// <param name="state">State of response</param>
        public ResponseStartField(IStartField value, StateCode state)
        {
            Value = value;
            State = state;
        }
    }
}
