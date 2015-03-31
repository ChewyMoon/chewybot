using ChewyBot.Input;

namespace ChewyBot.Interface
{
    /// <summary>
    ///     This interface must be inherited by any plugin.
    /// </summary>
    public interface IChewyBotPlugin
    {
        /// <summary>
        ///     Gets the commands this plugin should handle.
        /// </summary>
        /// <returns>Array of commands this plugin handles.</returns>
        string[] GetCommands();

        /// <summary>
        ///     Handles the command.
        /// </summary>
        /// <param name="args">The arguements of the command. The first item in the array([0]) is the command.</param>
        /// <returns><see cref="CommandResult" />. This can use bitwise operations.</returns>
        CommandResult HandleCommand(string[] args);
    }
}