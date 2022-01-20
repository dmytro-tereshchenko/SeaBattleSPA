using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
