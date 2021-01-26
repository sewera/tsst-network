using ClientNode.Config;
using ClientNode.Config.Parsers;
using ClientNode.Ui;
using ClientNode.Ui.Parsers;
using Common.Config.Parsers;
using Common.Models;
using Common.Networking.Client.Persistent;
using Common.Startup;
using Common.Ui;

namespace ClientNode
{
    public class ClientNode
    {
        public static void Main(string[] args)
        {
            DefaultStartup<ClientNode> defaultStartup = new DefaultStartup<ClientNode>();
            defaultStartup.InitArgumentParse(args);

            IConfigurationParser<Configuration> configurationParser;
            if (defaultStartup.ChooseXmlParser())
                configurationParser = new XmlConfigurationParser(defaultStartup.Filename);
            else
                configurationParser = new MockConfigurationParser();

            Configuration configuration = configurationParser.ParseConfiguration();

            defaultStartup.InitLogger(configuration.ClientAlias);

            ICommandParser commandParser = new CommandParser(configuration);
            IPersistentClientPort<MplsPacket> clientPort = new PersistentClientPort<MplsPacket>(configuration.ClientPortAlias,
                configuration.CableCloudAddress, configuration.CableCloudPort);
            IClientNodeManager clientNodeManager = new ClientNodeManager(configuration,
                clientPort,
                packet => new GenericPacket.Builder().SetType(GenericPacket.PacketType.Response).SetData(packet.Data).Build());
            // TODO: This is only a mock delegate

            IUserInterface userInterface = new UserInterface(commandParser, clientNodeManager);

            defaultStartup.SetTitle(configuration.ClientAlias);

            userInterface.Start();
        }
    }
}
