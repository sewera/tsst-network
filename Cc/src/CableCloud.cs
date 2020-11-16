using System;
using Cc.Cmd;
using Cc.Cmd.Parsers;
using Cc.Config;
using Cc.Networking.Forwarders;
using Cc.Networking.Listeners;
using NLog;

namespace Cc
{
    internal class CableCloud
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private Configuration _configuration;
        private IListener _listener;
        private IPacketForwarder _packetForwarder;
        private readonly ICommandParser _commandParser;

        private CableCloud(Configuration configuration,
                           IListener listener,
                           IPacketForwarder packetForwarder,
                           ICommandParser commandParser)
        {
            _configuration = configuration;
            _listener = listener;
            _packetForwarder = packetForwarder;
            _commandParser = commandParser;
        }

        public void Start()
        {
            LOG.Info("CableCloud started");
            _listener.Listen();
            StartCommandParsing();
        }

        public void StartCommandParsing()
        {
            LOG.Debug("Started command parsing, one command per line");
            while (true)
            {
                string input = Console.ReadLine();
                Command command = _commandParser.ParseCommand(input);
                switch (command.CommandType)
                {
                    case CommandType.SEND:
                        // TODO: Send to client
                        break;
                    default:
                        break;
                }
            }
        }

        public class Builder
        {
            private Configuration _configuration;
            private IListener _listener;
            private IPacketForwarder _packetForwarder;
            private ICommandParser _commandParser;

            public Builder SetConfiguration(Configuration configuration)
            {
                _configuration = configuration;
                return this;
            }

            public Builder SetListener(IListener listener)
            {
                _listener = listener;
                return this;
            }

            public Builder SetPacketForwarder(IPacketForwarder packetForwarder)
            {
                _packetForwarder = packetForwarder;
                return this;
            }

            public Builder SetCommandParser(ICommandParser commandParser)
            {
                _commandParser = commandParser;
                return this;
            }

            public CableCloud Build()
            {
                return new CableCloud(_configuration,
                    _listener,
                    _packetForwarder,
                    _commandParser);
            }
        }
    }
}
