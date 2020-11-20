using System;
using cn.Ui.Parsers;
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

        public (string, string) EnterReceiverAndMessage()
        {
            LOG.Info("Enter alias of remote host and message you want to send.\nInput format: <<port_serial_no>> [space] <<message>>");

            string input = Console.ReadLine();
            if (input == null) throw new Exception("Wrong command");
            string[] parts = input.Split(' ', 2);
            return (parts[0], parts[1]);
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Enter alias of remote host and message you want to send.\nInput format: <<port_serial_no>> [space] <<message>>");
                Console.Write("> ");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    LOG.Error("Input cannot be null nor empty");
                    continue;
                }

                (string destinationPortAlias, string message) = _commandParser.ParseCommand(input);
                _clientNodeManager.Send(destinationPortAlias, message);
            }
        }
    }
}

