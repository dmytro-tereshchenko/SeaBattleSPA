using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Enum possible states of the game.
    /// </summary>
    public enum GameState
    {
        Created, //Game was created
        SearchPlayers, //Waiting for searching and connecting players, after connect amount of maxPlayers next state Init
        Init, //Initializing game
        Process, //Game in process
        Finished //Game was finished
    }
}
