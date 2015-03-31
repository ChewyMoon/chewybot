using System;
using System.IO;
using System.Linq;
using System.Text;
using ChewyBot.Input;

namespace ChewyBot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!Directory.Exists("Plugins"))
            {
                Directory.CreateDirectory("Plugins");
            }

            if (args.Length != 0)
            {
                HandleCommand(args);
            }
            else
            {
                StartTerminal();
            }

#if DEBUG
            Console.ReadKey();
#endif
        }

        private static void StartTerminal()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("ChewyBot >  ");
                Console.ResetColor();

                var input = Console.ReadLine();

                if (input == null)
                {
                    continue;
                }

                HandleCommand(input.Split(' '));
            }
        }

        private static void HandleCommand(string[] args)
        {
            var status = Command.ProcessCommand(args);
            var builder = new StringBuilder();
            builder.Append("\nThe command returned: ");

            foreach (
                var flag in Enum.GetValues(typeof(CommandResult)).OfType<CommandResult>().Where(x => status.HasFlag(x)))
            {
                builder.Append(flag + " ");
            }

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(builder + "\n");
            Console.ResetColor();
        }
    }
}