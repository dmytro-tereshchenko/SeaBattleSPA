using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Infrastructure
{
    public interface IResponseGameField : IResponse
    {
        public IGameField Value { get; }
    }
}
