using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// The direction of the ship's position relative to the ship's stern.
    /// </summary>
    public enum DirectionOfShipPosition
    {
        /// <summary>
        /// X increase
        /// </summary>
        XInc,

        /// <summary>
        /// X decrease
        /// </summary>
        XDec,

        /// <summary>
        /// Y increase
        /// </summary>
        YInc,

        /// <summary>
        /// Y decrease
        /// </summary>
        YDec
    }
}
