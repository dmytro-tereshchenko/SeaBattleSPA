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
        void ShowGameField(IGameField field, ICollection<IGamePlayer> players, bool[,] startFieldLabels = null, IGamePlayer player = null,
            bool clear = true);

        /// <summary>
        /// Show menu
        /// </summary>
        /// <param name="canCancel">Can quit with press button "Escape"</param>
        /// <param name="message">Output previous message</param>
        /// <param name="action">Action which call in menu</param>
        /// <param name="options">Options of menu</param>
        /// <returns>result of choice</returns>
        int MenuMultipleChoice(bool canCancel, string message, Action action, params string[] options);

        /// <summary>
        /// The select cell of game field with keyboard's helping
        /// <para><see cref="ConsoleKey.Enter"/> - select cell</para>
        /// <para><see cref="ConsoleKey.LeftArrow"/> - move cursor left</para>
        /// <para><see cref="ConsoleKey.RightArrow"/> - move cursor right</para>
        /// <para><see cref="ConsoleKey.UpArrow"/> - move cursor up</para>
        /// <para><see cref="ConsoleKey.DownArrow"/> - move cursor down</para>
        /// <para><see cref="ConsoleKey.Escape"/> - escape without select</para>
        /// </summary>
        /// <param name="field"><see cref="IGameField"/></param>
        /// <param name="players">Collection of players in game</param>
        /// <param name="startPoint">Coordinates of start cursors' position</param>
        /// <param name="startFieldLabels">Array labels for game field when the player can put his own ships on start field</param>
        /// <param name="additionInfo">Calling the display of additional information after the playing field</param>
        /// <param name="player">Current player</param>
        /// <param name="clear">true - clear previous output, false - no action</param>
        /// <returns>(<see cref="ushort"/> X, <see cref="ushort"/> Y, <see cref="bool"/> Select) where X, Y - selected coordinates, Select - true - isSelected, otherwise exit</returns>
        (ushort X, ushort Y, bool Select) SelectCell(IGameField field, ICollection<IGamePlayer> players,
            (ushort X, ushort Y) startPoint, bool[,] startFieldLabels = null, Action additionInfo = null,
            IGamePlayer player = null, bool clear = true);
    }
}
