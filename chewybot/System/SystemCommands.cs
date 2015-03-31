using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ChewyBot.Input;
using ChewyBot.Interface;

namespace ChewyBot.System
{
    internal class SystemCommands : IChewyBotPlugin
    {
        public string[] GetCommands()
        {
            return new[] { "help", "set-option" };
        }

        public CommandResult HandleCommand(string[] args)
        {
            var cmd = args[0].ToLower();

            switch (cmd)
            {
                case "help":
                    if (args.Length == 2)
                    {
                        return GetHelpOnCommand(args[1]);
                    }

                    PrintHelpMessage();
                    return CommandResult.Success;

                case "set-option":
                    if (args.Length != 3)
                    {
                        Console.WriteLine(GetHelp("set-option"));
                        return CommandResult.Success;
                    }

                    Options.Instance[args[1]] = args[2];

                    break;
            }

            return CommandResult.Unknown;
        }

        public string GetHelp(string command)
        {
            if (command.ToLower().Equals("set-option"))
            {
                return @"Usage: set-option [option] [value]
	option -> the option to set
	value -> the value of the option to set";
            }

            if (command.ToLower() == "help")
            {
                PrintHelpMessage();
            }

            return string.Empty;
        }

        private static void PrintHelpMessage()
        {
            Console.WriteLine(@"Usage: help [command]
	command -> the command to get help on
	
	
	System Commands:
	
	set-option [option] [value]
		option -> the option to set
		value -> the value of the option to set");
        }

        private static CommandResult GetHelpOnCommand(string command)
        {
            try
            {
                // Find assembly that has that command
                foreach (var file in Directory.GetFiles("Plugins").Where(x => x.EndsWith(".dll")))
                {
                    var assembly = Assembly.LoadFrom(file);

                    var instances =
                        assembly.GetTypes()
                            .Where(
                                x =>
                                    x.GetInterfaces().Contains(typeof(IChewyBotPlugin)) &&
                                    x.GetConstructor(Type.EmptyTypes) != null)
                            .Select(x => Activator.CreateInstance(x) as IChewyBotPlugin);

                    foreach (var instance in
                        instances.Where(
                            instance => instance.GetCommands().Any(x => x.ToLower().Equals(command.ToLower()))))
                    {
                        Console.WriteLine(instance.GetHelp(command));
                        return CommandResult.Success;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nAn error occured trying to find help on that command. Stack Trace: ");
                Console.WriteLine(e);
                Console.WriteLine();

                return CommandResult.Error;
            }

            return CommandResult.Unknown;
        }
    }
}