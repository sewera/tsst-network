using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using nn.Config;
using nn.Config.Parsers;
using nn.Models;
using nn.Networking;
using nn.Networking.Client;
using nn.Networking.Forwarding;
using nn.Networking.Management;

namespace nn
{
    class NetworkNode
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            LogManager.Configuration = config;

            string filename = "";
            try
            {
                LOG.Trace($"Args: {string.Join(", ", args)}");
                if (args[0] == "-c")
                    filename = args[1];
                else if (args[1] == "-c")
                    filename = args[2];
                else
                    LOG.Warn("Use '-c <filename>' to pass a config file to program");
            }
            catch (IndexOutOfRangeException)
            {
                LOG.Warn("Use '-c <filename>' to pass a config file to program");
                LOG.Warn("Using MockConfigurationParser instead");
            }

            IConfigurationParser configurationParser;
            if (string.IsNullOrWhiteSpace(filename))
                configurationParser = new MockConfigurationParser();
            else
                configurationParser = new XmlConfigurationParser(filename);

            Configuration configuration = configurationParser.ParseConfiguration();

            IPacketForwarder packetForwarder = new MplsPacketForwarder(configuration);
            //IPacketForwarder packetForwarder = new MockPacketForwarder(configuration);
            IPort<ManagementPacket> managementPort = new ManagementPort(configuration);
            IClientPortFactory clientPortFactory = new ClientPortFactory(configuration);
            INetworkNodeManager networkNodeManager = new NetworkNodeManager(configuration, packetForwarder, managementPort, clientPortFactory);

            try
            {
                Console.Title = configuration.RouterAlias;
            }
            catch (Exception)
            {
                LOG.Trace("Could not set the title");
            }

            networkNodeManager.Start();
        }
    }
}
