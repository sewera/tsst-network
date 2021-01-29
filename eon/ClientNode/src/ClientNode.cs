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

            CpccState cpccState = new CpccState(configuration.NccConnectionRequestRemoteAddress,
                configuration.NccConnectionRequestRemotePort,
                configuration.NccCallTeardownRemoteAddress,
                configuration.NccCallTeardownRemotePort);

            ICommandParser commandParser = new CommandParser(configuration);
            IPersistentClientPort<EonPacket> clientPort = new PersistentClientPort<EonPacket>(configuration.ClientPortAlias,
                configuration.CableCloudAddress, configuration.CableCloudPort);
            IClientNodeManager clientNodeManager = new ClientNodeManager(configuration,
                clientPort,
                cpccState.OnCallAccept);

            IUserInterface userInterface = new UserInterface(commandParser, clientNodeManager, cpccState, configuration.ClientAlias);

            defaultStartup.SetTitle(configuration.ClientAlias);

            userInterface.Start();
        }
    }
}
