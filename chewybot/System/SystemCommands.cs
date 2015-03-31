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
                    break;
                case "set-option":
                    break;
            }

            return CommandResult.Unknown;
        }
    }
}