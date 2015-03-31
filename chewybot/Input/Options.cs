using System.Collections.Generic;

namespace ChewyBot.Input
{
    /// <summary>
    ///     Holds options.
    /// </summary>
    public class Options
    {
        private static Options _instance;
        private readonly Dictionary<string, string> _optionsDictionary = new Dictionary<string, string>();

        /// <summary>
        ///     Get the instance of the options.
        /// </summary>
        public static Options Instance
        {
            get { return _instance ?? (_instance = new Options()); }
        }

        /// <summary>
        ///     Gets/sets the option.
        /// </summary>
        /// <param name="option">The option to set.</param>
        /// <returns>The option stored.</returns>
        public string this[string option]
        {
            get { return _optionsDictionary[option]; }
            set { _optionsDictionary[option] = value; }
        }
    }
}