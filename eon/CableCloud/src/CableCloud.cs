using System;
using CableCloud.Config;
using CableCloud.Config.Parsers;
using CableCloud.Networking.Forwarding;
using CableCloud.Ui;
using CableCloud.Ui.Parsers;
using Common.Config.Parsers;
using Common.Models;
using Common.Networking.Server.Persistent;
using Common.Startup;
using Common.Ui;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace CableCloud
{
    public class CableCloud
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            DefaultStartup<CableCloud> defaultStartup = new DefaultStartup<CableCloud>();
            defaultStartup.InitArgumentParse(args);

            IConfigurationParser<Configuration> configurationParser;
            if (defaultStartup.ChooseXmlParser())
                configurationParser = new XmlConfigurationParser(defaultStartup.Filename);
            else
                configurationParser = new MockConfigurationParser();

            Configuration configuration = configurationParser.ParseConfiguration();

            defaultStartup.InitLogger(null);

            IWorkerFactory<MplsPacket> clientWorkerFactory = new WorkerFactory<MplsPacket>();
            IPersistentServerPort<MplsPacket> serverPort = new PersistentServerPort<MplsPacket>(configuration.ListeningAddress, configuration.ListeningPort, clientWorkerFactory);
            IPacketForwarder packetForwarder = new PacketForwarder(configuration);

            ICableCloudManager cableCloudManager = new CableCloudManager(serverPort, packetForwarder);

            ICommandParser commandParser = new CommandParser(configuration);
            IUserInterface userInterface = new UserInterface(commandParser, cableCloudManager);

            try
            {
                Console.Title = "CC";
            }
            catch (Exception)
            {
                LOG.Trace("Could not set the title");
            }

            userInterface.Start();
        }
    }
}
