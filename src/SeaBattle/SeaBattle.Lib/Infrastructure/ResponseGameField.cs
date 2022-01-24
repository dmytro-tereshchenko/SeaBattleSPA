using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Infrastructure
{
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
