using System;
using System.Threading;
using ClientNetwork.Models;
using ClientNetwork.Ui.Parsers;
using ClientNetwork.Ui.Parsers.Exceptions;
using NLog;

namespace ClientNetwork.Ui
{
    public class UserInterface : IUserInterface
    {
        private readonly ICommandParser _commandParser;
        private readonly IClientNodeManager _clientNodeManager;
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        public UserInterface(ICommandParser commandParser, IClientNodeManager clientNodeManager)
        {
            _commandParser = commandParser;
            _clientNodeManager = clientNodeManager;
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
            Console.WriteLine("Enter MPLS out label to direct packet to specific remote host and message you want to send.\n" +
                              "Input format: <<mpls_out_label>> [space] <<message>>");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                try
                {
                    (string mplsOutLabel, string message) = _commandParser.ParseCommand(input); 
                    _clientNodeManager.Send(mplsOutLabel, message, _commandParser.CheckMplsOutLabel(mplsOutLabel));
                }
                catch (ParserException e)
                {
                    LOG.Warn(e.ExceptionMessage);
                }
            }
        }

        private static void MessageReceived(MplsPacket mplsPacket)
        {
            Console.WriteLine($"Received: {mplsPacket}");
            Console.Write("> ");
        }
    }
}
