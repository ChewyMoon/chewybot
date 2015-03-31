using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ChewyBot.Interface;

namespace ChewyBot.Input
{
    /// <summary>
    ///     This class handles commands given by the user, or executes commands.
    /// </summary>
    public class Command
    {
        /// <summary>
        ///     Commands that are included by default in the system.
        /// </summary>
        public static string[] SystemCommands = { "help", "set" };

        /// <summary>
        ///     Processes the command given.
        /// </summary>
        /// <param name="args">Command + arguements</param>
        /// <returns><see cref="CommandResult" />. This can return a bitwise operation of results.</returns>
        public static CommandResult ProcessCommand(string[] args)
        {
            try
            {
                // Check if the command is a system command, and handle it.
                if (SystemCommands.Any(x => Equals(SystemCommands, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return ExecuteCommand(Assembly.GetExecutingAssembly(), args);
                }


                foreach (var file in Directory.GetFiles("Plugins").Where(x => x.EndsWith(".dll")))
                {
                    return ExecuteCommand(Assembly.LoadFrom(file), args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nThere was an error trying to process this commmand! Stack trace: ");
                Console.WriteLine(e);
                Console.WriteLine();

                return CommandResult.Error;
            }


            return CommandResult.Unknown;
        }

        private static CommandResult ExecuteCommand(Assembly assembly, string[] args)
        {
            var cmd = args[0];

            var instances =
                assembly.GetTypes()
                    .Where(
                        x =>
                            x.GetInterfaces().Contains(typeof(IChewyBotPlugin)) &&
                            x.GetConstructor(Type.EmptyTypes) != null)
                    .Select(x => Activator.CreateInstance(x) as IChewyBotPlugin);

            return
                instances.Where(instance => instance.GetCommands().Contains(cmd))
                    .Select(instance => instance.HandleCommand(args))
                    .FirstOrDefault();
        }
    }

    /// <summary>
    ///     The result of the command being executed.
    /// </summary>
    [Flags]
    public enum CommandResult
    {
        /// <summary>
        ///     The status of the command is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     The command executed sucessfully.
        /// </summary>
        Success = 2,

        /// <summary>
        ///     A warning occured executing the command.
        /// </summary>
        Warning = 4,

        /// <summary>
        ///     There was an error executing the command.
        /// </summary>
        Error = 8
    }
}