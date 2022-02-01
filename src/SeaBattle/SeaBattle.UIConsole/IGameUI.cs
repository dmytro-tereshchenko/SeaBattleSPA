using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.UIConsole
{
    /// <summary>
    /// Game "Sea battle"
    /// </summary>
    public interface IGameUI
    {
        /// <summary>
        /// Start the game (creating all entities and actions during the game.
        /// </summary>
        void Start();
    }
}
