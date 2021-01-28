using Common.Config.Parsers;
using Common.Models;
using Common.Startup;
using RoutingController.Config;
using RoutingController.Config.Parsers;

namespace RoutingController
{
    public class RoutingController
    {
        public static void Main(string[] args)
        {
            DefaultStartup<RoutingController> defaultStartup = new DefaultStartup<RoutingController>();
            defaultStartup.InitArgumentParse(args);
            IConfigurationParser<Configuration> configurationParser;

            if (defaultStartup.ChooseXmlParser())
                configurationParser = new XmlConfigurationParser(defaultStartup.Filename); 
            else
                configurationParser = new MockConfigurationParser();

            defaultStartup.InitLogger(null);

            Configuration configuration = configurationParser.ParseConfiguration();

            IRcState rcState = new RcState(); // Add some data from config

            IManager routingControllerManager = new RoutingControllerManager(configuration,
                rcState.OnRouteTableQuery,
                rcState.OnLocalTopology,
                rcState.OnNetworkTopology);
            routingControllerManager.Start();
        }
    }
}
