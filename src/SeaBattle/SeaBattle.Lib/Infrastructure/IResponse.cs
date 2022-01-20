using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Infrastructure
{
    public interface IResponse
    {
        StateCode State { get; }
    }
}
