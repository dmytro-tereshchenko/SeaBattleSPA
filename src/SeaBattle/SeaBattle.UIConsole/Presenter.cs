using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public void ShowMessage(string message, bool clear = true)
        {
            if (clear)
            {
                Console.Clear();
            }

            Console.WriteLine(message);

            Console.Write("Press button...");
            Console.ReadLine();
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

        public void ShowGameField(IGameField field, ICollection<IGamePlayer> players, IGamePlayer player = null, bool clear = true)
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
                    if (field[i, j] != null && player == field[i, j].GamePlayer)
                    {
                        Console.BackgroundColor = (ConsoleColor) 1;
                        PrintSymbol(' ', sizeNumberY);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (field[i, j] != null && player == null)
                    {
                        Console.BackgroundColor = (ConsoleColor) (playersList.IndexOf(field[i, j].GamePlayer) + 1);
                        PrintSymbol(' ', sizeNumberY);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        PrintSymbol(' ', sizeNumberY);
                    }
                }

                Console.WriteLine("|");
            }

            //Output horizontal border
            PrintSymbol(' ', sizeNumberX);
            Console.Write('+');
            PrintSymbol('-', sizeNumberY * field.SizeY);
            Console.WriteLine('+');
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
