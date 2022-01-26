using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Types of possible interaction between ships
    /// </summary>
    public enum TypeOfAction
    {
        /// <summary>
        /// Attack target ship (reduce his hp)
        /// </summary>
        Attack,

        /// <summary>
        /// Repair target ship/ships (restore his/them hp)
        /// </summary>
        Repair
    }
}
