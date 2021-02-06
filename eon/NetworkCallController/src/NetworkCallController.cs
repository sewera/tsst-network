using Common.Config.Parsers;
using Common.Models;
using Common.Startup;
using NetworkCallController.Config;
using NetworkCallController.Config.Parsers;
using NetworkNode.Config.Parsers;

namespace NetworkCallController
{
    public class NetworkCallController
    {
        public static void Main(string[] args)
        {
            DefaultStartup<NetworkCallController> defaultStartup = new DefaultStartup<NetworkCallController>();
            defaultStartup.InitArgumentParse(args);
            IConfigurationParser<Configuration> configurationParser;

            if (defaultStartup.ChooseXmlParser())
                configurationParser = new XmlConfigurationParser(defaultStartup.Filename);
            else
                configurationParser = new MockConfigurationParser();

            defaultStartup.InitLogger(null); // TODO: Set log suffix from configuration
            
            Configuration configuration = configurationParser.ParseConfiguration();

            NccState nccState = new NccState(configuration.ClientPortAliases,
                configuration.PortDomains,
                configuration.Domain,
                configuration.ServerAddress,
                configuration.ConnectionRequestRemotePort,
                configuration.CallCoordinationRemotePort);

            IManager networkCallControllerManager = new NetworkCallControllerManager(configuration,
                nccState.OnCallCoordinationReceived,
                nccState.OnCallTeardownReceived,
                nccState.OnConnectionRequestReceived);
            // TODO: Those are only empty delegates, make proper ones: 2 left

            defaultStartup.SetTitle("NCC_" + configuration.Domain);

            networkCallControllerManager.Start();
        }
    }
}
