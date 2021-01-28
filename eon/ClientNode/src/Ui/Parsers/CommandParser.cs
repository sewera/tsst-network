using System;
using System.Collections.Generic;
using ClientNode.Config;
using ClientNode.Ui.Parsers.Exceptions;
using NLog;

namespace ClientNode.Ui.Parsers
{
    public class CommandParser : ICommandParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly Configuration _configuration;
        
        public CommandParser(Configuration configuration)
        {
            _configuration = configuration;
        }

        public (string, int) ParseCpccCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ParserException("Command cannot be empty");
            try
            {
                string[] parts = command.Split(' ', 2);
                if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
                    throw new ParserException("Command parts cannot be neither null nor empty");
                int slotsNumber = int.Parse(parts[1]);
                return (parts[0], slotsNumber);
            }
            catch (Exception)
            {
                throw new ParserException("Wrong command");
            }
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
        
        public (List<long>, string) CheckMplsOutLabel(string mplsOutLabel)
        {
            List<long> mplsLabels = new List<long>();
            string remoteHostAlias = String.Empty;
            try
            {
                foreach((string alias, long label) in _configuration.MplsLabels)
                {
                    if (long.Parse(mplsOutLabel) == label)
                    {
                        mplsLabels.Add(long.Parse(mplsOutLabel));
                        remoteHostAlias = alias;
                    }
                }
            }
            catch (FormatException)
            {
                throw new ParserException("Could not find given MPLS label");
            }
            return (mplsLabels, remoteHostAlias);
        }
    }
}
