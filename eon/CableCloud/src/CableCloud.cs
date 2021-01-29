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

namespace CableCloud
{
    public class CableCloud
    {
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

            IWorkerFactory<EonPacket> clientWorkerFactory = new WorkerFactory<EonPacket>();
            IPersistentServerPort<EonPacket> serverPort = new PersistentServerPort<EonPacket>(configuration.ListeningAddress, configuration.ListeningPort, clientWorkerFactory);
            IPacketForwarder packetForwarder = new PacketForwarder(configuration);

            ICableCloudManager cableCloudManager = new CableCloudManager(serverPort, packetForwarder);

            ICommandParser commandParser = new CommandParser(configuration);
            IUserInterface userInterface = new UserInterface(commandParser, cableCloudManager);

            defaultStartup.SetTitle("CC");

            userInterface.Start();
        }
    }
}
