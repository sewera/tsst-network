using System;
using Common.Config.Parsers;
using Common.Models;
using Common.Networking.Client.Persistent;
using Common.Startup;
using NLog;
using NLog.Config;
using NLog.Targets;
using NetworkNode.Config;
using NetworkNode.Config.Parsers;
using NetworkNode.Networking.Forwarding;

namespace NetworkNode
{
    class NetworkNode
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            DefaultStartup<NetworkNode> defaultStartup = new DefaultStartup<NetworkNode>();
            defaultStartup.InitArgumentParse(args);

            IConfigurationParser<Configuration> configurationParser;
            if (defaultStartup.ChooseXmlParser())
                configurationParser = new XmlConfigurationParser(defaultStartup.Filename);
            else
                configurationParser = new MockConfigurationParser();

            Configuration configuration = configurationParser.ParseConfiguration();

            defaultStartup.InitLogger(configuration.RouterAlias);

            IPacketForwarder packetForwarder = new MplsPacketForwarder();
            //IPacketForwarder packetForwarder = new MockPacketForwarder(configuration);
            IPersistentClientPortFactory<MplsPacket> clientPortFactory = new PersistentClientPortFactory<MplsPacket>(configuration.CableCloudAddress, configuration.CableCloudPort);
            IManager networkNodeManager = new NetworkNodeManager(configuration, packetForwarder, clientPortFactory);

            defaultStartup.SetTitle(configuration.RouterAlias);

            networkNodeManager.Start();
        }
    }
}
