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
        /// <param name="clear">true - clear previous output, false - no action</param>
        /// <param name="pause">true - pause for read message, false - no action</param>
        void ShowMessage(string message, bool clear = true, bool pause = true);

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
        /// <param name="startFieldLabels">Array labels for game field when the player can put his own ships on start field</param>
        void ShowGameField(IGameField field, ICollection<IGamePlayer> players, IGamePlayer player = null,
            bool clear = true, bool[,] startFieldLabels = null);

        /// <summary>
        /// Show menu
        /// </summary>
        /// <param name="canCancel">Can quit with press button "Escape"</param>
        /// <param name="message">Output previous message</param>
        /// <param name="action">Action which call in menu</param>
        /// <param name="options">Options of menu</param>
        /// <returns>result of choice</returns>
        int MenuMultipleChoice(bool canCancel, string message, Action action, params string[] options);
    }
}
