using Common.Config.Parsers;
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

            IRcState rcState = new RcState(configuration.RouteTable);
            
            defaultStartup.SetTitle($"RC_{configuration.ComponentName}");

            IManager routingControllerManager = new RoutingControllerManager(configuration,
                rcState.OnRouteTableQuery,
                rcState.OnLocalTopology,
                rcState.OnNetworkTopology);
            routingControllerManager.Start();
        }
    }
}
