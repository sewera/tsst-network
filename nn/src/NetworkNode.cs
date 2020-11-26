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

            // IConfigurationParser configurationParser = new XmlConfigurationParser("resources/configuration.xml");
            IConfigurationParser configurationParser = new MockConfigurationParser();
            Configuration configuration = configurationParser.ParseConfiguration();

            IPacketForwarder packetForwarder = new MplsPacketForwarder(configuration);
            //IPacketForwarder packetForwarder = new MockPacketForwarder(configuration);
            IPort<ManagementPacket> managementPort = new ManagementPort(configuration);
            IClientPortFactory clientPortFactory = new ClientPortFactory(configuration);
            INetworkNodeManager networkNodeManager = new NetworkNodeManager(configuration, packetForwarder, managementPort, clientPortFactory);

            networkNodeManager.Start();
        }
    }
}
