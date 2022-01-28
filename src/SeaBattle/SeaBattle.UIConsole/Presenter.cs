using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.UIConsole
{
    /// <summary>
    /// Input, output data with user using the console.
    /// </summary>
    public class Presenter : IPresenter
    {
        public T GetData<T>(string message) where T : struct
        {
            bool result = false;
            string showMessage = message;
            T data = default(T);

            do
            {
                Console.Clear();
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

        public void ShowMessage(string message) => Console.WriteLine(message);

    }
}
