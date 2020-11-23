using System;
using NLog;
using nn.src.Ui.Parsers.Exceptions;

namespace nn.src.Ui.Parsers
{
    public class CommandParser : ICommandParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public (string, string) ParseCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ParserException("Command cannot be empty");
            try
            {
                string[] parts = command.Split(' ', 2);
                if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
                    throw new ParserException("Command parts cannot be neither null nor empty");
                return (parts[0], parts[1]);
            }
            catch (Exception e)
            {
                throw new ParserException("Wrong command");
            }
        }
    }
}
