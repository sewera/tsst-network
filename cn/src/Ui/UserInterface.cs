using System;
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

            Console.WriteLine("Enter alias of remote host and message you want to send.\nInput format: <<port_serial_no>> [space] <<message>>");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                try
                {
                    (string destinationPortAlias, string message) = _commandParser.ParseCommand(input);
                    _clientNodeManager.Send(destinationPortAlias, message);
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
        }
    }
}
