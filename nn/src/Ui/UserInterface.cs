using System;
using System.Threading;
using NLog;
using nn.src.Models;
using nn.src.Ui.Parsers;
using nn.src.Ui.Parsers.Exceptions;

namespace nn.src.Ui
{
    public class UserInterface : IUserInterface
    {
        private readonly ICommandParser _commandParser;
        private readonly INetworkNodeManager _networkNodeManager;
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        public UserInterface(ICommandParser commandParser, INetworkNodeManager networkNodeManager)
        {
            _commandParser = commandParser;
            _networkNodeManager = networkNodeManager;
        }

        public void Start()
        {
            _networkNodeManager.Start();
            _networkNodeManager.RegisterReceiveMessageEvent(MessageReceived);
            ThreadStart startCommandParsing = StartCommandParsing;
            LOG.Trace("Creating child thread to handle command parsing");
            Thread commandParsingThread = new Thread(startCommandParsing);
            LOG.Trace("Starting command parsing thread");
            commandParsingThread.Start();
            LOG.Trace("Started thread");
        }

        private void StartCommandParsing()
        {
            Console.WriteLine("Enter alias of remote host and message you want to send.\nInput format: <<port_serial_no>> [space] <<message>>");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                try
                {
                    (string destinationPortAlias, string message) = _commandParser.ParseCommand(input);
                    _networkNodeManager.Send(destinationPortAlias, message);
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
