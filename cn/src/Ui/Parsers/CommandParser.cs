using System;
using cn.Models;
using cn.Ui.Parsers.Exceptions;
using NLog;

namespace cn.Ui.Parsers
{
    public class CommandParser : ICommandParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public (string, string) ParseCommand(string command)
        {
            string[] parts = command.Split(' ', 2);
            if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
                throw new ParserException("Command parts cannot be neither null nor empty");
            return (parts[0], parts[1]);
        }
    }
}
