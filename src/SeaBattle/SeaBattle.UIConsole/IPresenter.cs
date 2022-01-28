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
        /// <returns>struct</returns>
        T GetData<T>(string message) where T : struct;

        /// <summary>
        /// Output message
        /// </summary>
        /// <param name="message">Message</param>
        void ShowMessage(string message);
    }
}
