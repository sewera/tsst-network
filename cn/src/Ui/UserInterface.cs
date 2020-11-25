using System;
using System.Collections.Generic;
using System.Threading;
using cn.Config;
using cn.Models;
using cn.Ui.Parsers;
using cn.Ui.Parsers.Exceptions;
using NLog;

namespace cn.Ui
{
    public class UserInterface : IUserInterface
    {
        private readonly ICommandParser _commandParser;
        private readonly IClientNodeManager _clientNodeManager;
        private readonly Configuration _configuration;
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        public UserInterface(ICommandParser commandParser, IClientNodeManager clientNodeManager, Configuration configuration)
        {
            _commandParser = commandParser;
            _clientNodeManager = clientNodeManager;
            _configuration = configuration;
        }

        public void Start()
        {
            _clientNodeManager.Start();
            _clientNodeManager.RegisterReceiveMessageEvent(MessageReceived);
            ThreadStart startCommandParsing = StartCommandParsing;
            LOG.Trace("Creating child thread to handle command parsing");
            Thread commandParsingThread = new Thread(startCommandParsing);
            LOG.Trace("Starting command parsing thread");
            commandParsingThread.Start();
            LOG.Trace("Started thread");
        }

        private void StartCommandParsing()
        {
            Console.WriteLine("Enter alias of remote host and message you want to send.\nInput format: <<remote_host_alias>> [space] <<message>>");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                try
                {
                    (string remoteHostAlias, string message) = _commandParser.ParseCommand(input);
                    _clientNodeManager.Send(remoteHostAlias.ToUpper(), message, SelectOutLabel(remoteHostAlias.ToUpper()));
                }
                catch (ParserException e)
                {
                    LOG.Warn(e.ExceptionMessage);
                }
            }
        }

        private List<long> SelectOutLabel(string remoteHostAlias)
        {
            List<long> mplsLabels = new List<long>();
            try
            {
                mplsLabels.Add(_configuration.MplsLabels[remoteHostAlias]);
            }
            catch (KeyNotFoundException e)
            {
                throw new ParserException("Could not find any matching MPLS label for given remote client node alias");
            }
            return mplsLabels;
        }

        private static void MessageReceived(MplsPacket mplsPacket)
        {
            Console.WriteLine($"Received: {mplsPacket}");
            Console.Write("> ");
        }
    }
}
