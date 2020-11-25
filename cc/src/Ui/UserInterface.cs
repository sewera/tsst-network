using System;
using System.Threading;
using cc.Ui.Parsers;
using cc.Ui.Parsers.Exceptions;
using NLog;

namespace cc.Ui
{
    public class UserInterface : IUserInterface
    {
        private readonly ICommandParser _commandParser;
        private readonly ICableCloudManager _cableCloudManager;
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        public UserInterface(ICommandParser commandParser, ICableCloudManager cableCloudManager)
        {
            _commandParser = commandParser;
            _cableCloudManager = cableCloudManager;
        }

        public void Start()
        {
            _cableCloudManager.Start();
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
                    (string, string, bool) connection = _commandParser.ParseCommand(input);
                    _cableCloudManager.SetConnectionAlive(connection);
                }
                catch (ParserException e)
                {
                    LOG.Warn(e.ExceptionMessage);
                }
            }
        }
    }
}
