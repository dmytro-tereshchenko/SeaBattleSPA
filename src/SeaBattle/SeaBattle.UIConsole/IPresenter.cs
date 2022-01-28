using SeaBattle.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.UIConsole
{
    /// <summary>
    /// Input, output data with user
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// Input data
        /// </summary>
        /// <typeparam name="T">struct</typeparam>
        /// <param name="message">Output message before user input</param>
        /// <param name="clear">true - clear previous output, false - not action</param>
        /// <returns>struct</returns>
        T GetData<T>(string message, bool clear = true) where T : struct;

        /// <summary>
        /// Output message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="clear">true - clear previous output, false - not action</param>
        void ShowMessage(string message, bool clear = true);

        /// <summary>
        /// Input string
        /// </summary>
        /// <param name="message">Output message before user input</param>
        /// <param name="clear">true - clear previous output, false - not action</param>
        /// <returns><see cref="string"/> Input</returns>
        string GetString(string message, bool clear = true);

        /// <summary>
        /// Show game field
        /// </summary>
        /// <param name="field">Game field</param>
        /// <param name="players">Collection of players in game</param>
        /// <param name="player">Current player</param>
        /// <param name="clear">true - clear previous output, false - not action</param>
        void ShowGameField(IGameField field, ICollection<IGamePlayer> players, IGamePlayer player = null,
            bool clear = true);
    }
}
