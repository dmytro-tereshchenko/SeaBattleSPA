using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Infrastructure
{
    public enum StateCode
    {
        //OK
        Success = 010,

        //Not enough allocation points for buying the ship
        PointsShortage = 011,

        //Invalid playing field size
        InvalidFieldSize = 013,

        //Invalid Team, the team isn't matched with the field
        InvalidTeam = 014
    }
}
