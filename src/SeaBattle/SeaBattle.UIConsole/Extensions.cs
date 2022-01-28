using System;
using System.ComponentModel;

namespace SeaBattle.UIConsole
{
    /// <summary>
    /// Extensions for <see cref="GameUI"/>
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Generic Convert
        /// </summary>
        /// <typeparam name="T"><see cref="struct"/></typeparam>
        /// <param name="input"><see cref="string"/> which converted</param>
        /// <returns><paramref name="T"/></returns>
        public static T Convert<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(input);
                }
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }
    }
}
