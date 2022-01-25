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

        public ResponseGameField(IGameField value, StateCode state)
        {
            Value = value;
            State = state;
        }
    }
}
