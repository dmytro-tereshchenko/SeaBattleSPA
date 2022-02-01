using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SeaBattle.Lib.Entities;

namespace SeaBattle.UIConsole
{
    /// <summary>
    /// Input, output data with user using the console.
    /// </summary>
    public class Presenter : IPresenter
    {
        public T GetData<T>(string message, bool clear = true) where T : struct
        {
            bool result = false;
            string showMessage = message;
            T data = default(T);

            do
            {
                if (clear)
                {
                    Console.Clear();
                }
                Console.WriteLine(showMessage);

                try
                {
                    data = Console.ReadLine().Convert<T>();
                    result = true;

                }
                catch (ArgumentException ex)
                {
                    showMessage = "Wrong enter. You should repeat.\n" + message;
                }
            } while (!result);

            return data;
        }

        public void ShowMessage(string message, bool clear = true, bool pause = true, bool newLine = true,
            ConsoleColor color = ConsoleColor.White)
        {
            if (clear)
            {
                Console.Clear();
            }

            Console.ForegroundColor = color;

            if (newLine)
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.Write(message);
            }

            if (pause)
            {
                Console.Write("Press button...");
                Console.ReadLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public string GetString(string message, bool clear = true)
        {
            if (clear)
            {
                Console.Clear();
            }

            Console.WriteLine(message);

            return Console.ReadLine();
        }

        public void ShowGameField(IGameField field, ICollection<IGamePlayer> players, bool[,] startFieldLabels = null, IGamePlayer player = null, bool clear = true)
        {
            if (clear)
            {
                Console.Clear();
            }

            List<IGamePlayer> playersList = players.ToList();

            ushort sizeNumberY = GetSizeNumber(field.SizeY);
            ushort sizeNumberX = GetSizeNumber(field.SizeX);

            //Output first line
            PrintSymbol(' ', sizeNumberX + 1);
            for (ushort i = 1; i <= field.SizeY; i++)
            {
                PrintSymbol(' ', sizeNumberY-GetSizeNumber(i));
                Console.Write(i);
            }
            Console.WriteLine();

            //Output horizontal border
            PrintSymbol(' ', sizeNumberX);
            Console.Write('+');
            PrintSymbol('-', sizeNumberY * field.SizeY);
            Console.WriteLine('+');

            //Output field
            for (ushort i = 1; i <= field.SizeX; i++)
            {
                PrintSymbol(' ', sizeNumberX - GetSizeNumber(i));
                Console.Write(i + "|");

                for (ushort j = 1; j <= field.SizeY; j++)
                {
                    ShowCellOfField(field, new(i, j), playersList, startFieldLabels, player);
                }

                Console.WriteLine("|");
            }

            //Output horizontal border
            PrintSymbol(' ', sizeNumberX);
            Console.Write('+');
            PrintSymbol('-', sizeNumberY * field.SizeY);
            Console.WriteLine('+');
        }

        public int MenuMultipleChoice(bool canCancel, string message, Action action, params string[] options)
        {
            int optionsPerLine = options.Length;
            int currentSelection = 0;
            ConsoleKey key;
            Console.CursorVisible = false;
            do
            {
                action?.Invoke();

                if (message != null)
                {
                    Console.WriteLine(message);
                }

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == currentSelection)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.WriteLine(options[i]);

                    Console.ResetColor();
                }
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentSelection > 0)
                        {
                            currentSelection--;
                        }

                        break;
                    case ConsoleKey.DownArrow:
                        if (currentSelection < optionsPerLine - 1)
                        {
                            currentSelection++;
                        }

                        break;
                    case ConsoleKey.Escape:
                        if (canCancel)
                        {
                            return -1;
                        }

                        break;
                }
            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;
            return currentSelection;
        }

        public (ushort X, ushort Y, bool Select) SelectCell(IGameField field, ICollection<IGamePlayer> players,
            (ushort X, ushort Y) startPoint, bool[,] startFieldLabels = null, Action additionInfo = null,
            IGamePlayer player = null, bool clear = true)
        {
            ShowGameField(field, players, startFieldLabels, player, clear);
            additionInfo?.Invoke();

            ConsoleKeyInfo key;
            (ushort X, ushort Y) newPoint = new(startPoint.X, startPoint.Y);
            bool move = false;
            Console.CursorVisible = false;

            //show cursor
            SetCursorPosition(field, newPoint);
            ShowCursor(field);

            while (true)
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        Console.CursorVisible = true;
                        return new(startPoint.X, startPoint.Y, true);
                    case ConsoleKey.LeftArrow: //move cursor left
                        newPoint = new(startPoint.X, (ushort) (startPoint.Y - 1));
                        move = RewriteCursor(field, startPoint, newPoint, players.ToList(), startFieldLabels, player);
                        break;
                    case ConsoleKey.RightArrow: //move cursor right
                        newPoint = new(startPoint.X, (ushort) (startPoint.Y + 1));
                        move = RewriteCursor(field, startPoint, newPoint, players.ToList(), startFieldLabels, player);
                        break;
                    case ConsoleKey.UpArrow: //move cursor up
                        newPoint = new((ushort) (startPoint.X - 1), startPoint.Y);
                        move = RewriteCursor(field, startPoint, newPoint, players.ToList(), startFieldLabels, player);
                        break;
                    case ConsoleKey.DownArrow: //move cursor down
                        newPoint = new((ushort) (startPoint.X + 1), startPoint.Y);
                        move = RewriteCursor(field, startPoint, newPoint, players.ToList(), startFieldLabels, player);
                        break;
                    case ConsoleKey.Escape: //exit
                        Console.CursorVisible = true;
                        return new(startPoint.X, startPoint.Y, false);
                }

                if (move)
                {
                    //move coordinates of cell
                    startPoint.X = newPoint.X;
                    startPoint.Y = newPoint.Y;
                }
            }
        }

        /// <summary>
        /// Show Cell of <see cref="IGameField"/> by coordinates
        /// </summary>
        /// <param name="field"><see cref="IGameField"/></param>
        /// <param name="point">Coordinates of field's cell</param>
        /// <param name="playersList">Collection of players in game</param>
        /// <param name="startFieldLabels">Array labels for game field when the player can put his own ships on start field</param>
        /// <param name="player">Current player</param>
        private void ShowCellOfField(IGameField field, (ushort X, ushort Y) point, IList<IGamePlayer> playersList, bool[,] startFieldLabels = null, IGamePlayer player = null)
        {
            ushort sizeNumberY = GetSizeNumber(field.SizeY);

            if (field[point.X, point.Y] != null && player == field[point.X, point.Y].GamePlayer)
            {
                Console.BackgroundColor = (ConsoleColor)1;
                PrintSymbol(' ', sizeNumberY);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (field[point.X, point.Y] != null && player == null)
            {
                Console.BackgroundColor = (ConsoleColor)(playersList.IndexOf(field[point.X, point.Y].GamePlayer) + 1);
                PrintSymbol(' ', sizeNumberY);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if (startFieldLabels != null && startFieldLabels[point.X - 1, point.Y - 1])
            {
                //If show StartField than mark this zone
                Console.BackgroundColor = ConsoleColor.DarkGray;
                PrintSymbol(' ', sizeNumberY);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                PrintSymbol(' ', sizeNumberY);
            }
        }

        /// <summary>
        /// Show cursor on game field
        /// </summary>
        private void ShowCursor(IGameField field)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            PrintSymbol(' ', GetSizeNumber(field.SizeY));
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Rewrite changing position of the cursor
        /// </summary>
        /// <param name="field"><see cref="IGameField"/></param>
        /// <param name="oldPoint">Coordinates of old field's cell</param>
        /// <param name="newPoint">Coordinates of new field's cell</param>
        /// <param name="playersList">Collection of players in game</param>
        /// <param name="startFieldLabels">Array labels for game field when the player can put his own ships on start field</param>
        /// <param name="player">Current player</param>
        private bool RewriteCursor(IGameField field, (ushort X, ushort Y) oldPoint, (ushort X, ushort Y) newPoint,
            IList<IGamePlayer> playersList, bool[,] startFieldLabels = null, IGamePlayer player = null)
        {
            // if out of the game's field then return without changing
            if (newPoint.X < 1 || newPoint.Y < 1 || newPoint.X > field.SizeX || newPoint.Y > field.SizeY)
            {
                return false;
            }
            
            SetCursorPosition(field, oldPoint);
            ShowCellOfField(field, oldPoint, playersList, startFieldLabels, player);
            SetCursorPosition(field, newPoint);
            ShowCursor(field);

            return true;
        }

        /// <summary>
        /// Set cursor on game field in console by coordinates
        /// </summary>
        /// <param name="field"><see cref="IGameField"/></param>
        /// <param name="point">Coordinates of cursor's position</param>
        private void SetCursorPosition(IGameField field, (ushort X, ushort Y) point)
        {
            ushort sizeNumberY = GetSizeNumber(field.SizeY);
            ushort sizeNumberX = GetSizeNumber(field.SizeX);
            int posLeft = sizeNumberX + sizeNumberY * (point.Y - 1) + 1;
            int posTop = 1 + point.X;
            Console.SetCursorPosition(posLeft, posTop);
        }

        /// <summary>
        /// Get the degree of a number
        /// </summary>
        /// <param name="number">Number</param>
        /// <returns><see cref="ushort"/> degree of a number</returns>
        private ushort GetSizeNumber(ushort number) => (ushort) Math.Ceiling((decimal) Math.Log10(number + 1));

        /// <summary>
        /// Print in console line of symbols
        /// </summary>
        /// <param name="symbol">Symbol which print</param>
        /// <param name="count">String length</param>
        private void PrintSymbol(char symbol, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                Console.Write(symbol);
            }
        }

    }
}
