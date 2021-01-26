using System;
using CableCloud.Config;
using CableCloud.Ui.Parsers.Exceptions;
using NLog;

namespace CableCloud.Ui.Parsers
{
    public class CommandParser : ICommandParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly Configuration _configuration;
        
        public CommandParser(Configuration configuration)
        {
            _configuration = configuration;
        }

        public (string, string, bool) ParseCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ParserException("Command cannot be empty");
            try
            {
                string[] parts = command.Split(' ', 3);
                LOG.Trace($"Command parts: {string.Join(", ", parts)}");
                if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]) || string.IsNullOrEmpty(parts[2]))
                    throw new ParserException("Command parts cannot be neither null nor empty");
                return (parts[0], parts[1], parts[2].ToLower() == "up");
            }
            catch (Exception e)
            {
                LOG.Trace(e);
                throw new ParserException("Wrong command, syntax: 'port1 port2 [up|down]'");
            }
        }
    }
}
