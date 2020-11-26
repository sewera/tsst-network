using System;
using System.Collections.Generic;
using cn.Config;
using cn.Ui.Parsers.Exceptions;
using NLog;

namespace cn.Ui.Parsers
{
    public class CommandParser : ICommandParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly Configuration _configuration;
        
        public CommandParser(Configuration configuration)
        {
            _configuration = configuration;
        }

        public (string, string) ParseCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ParserException("Command cannot be empty");
            try
            {
                string[] parts = command.Split(' ', 2);
                if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
                    throw new ParserException("Command parts cannot be neither null nor empty");
                return (parts[0].ToUpper(), parts[1]);
            }
            catch (Exception)
            {
                throw new ParserException("Wrong command");
            }
        }
        
        public List<long> SelectOutLabel(string remoteHostAlias)
        {
            List<long> mplsLabels = new List<long>();
            try
            {
                mplsLabels.Add(_configuration.MplsLabels[remoteHostAlias]);
            }
            catch (KeyNotFoundException)
            {
                throw new ParserException("Could not find any matching MPLS label for given remote client node alias");
            }
            return mplsLabels;
        }
    }
}
