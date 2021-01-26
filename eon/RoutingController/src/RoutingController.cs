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
                configurationParser = new MockConfigurationParser(); // TODO: Change for XmlConfigurationParser
            else
                configurationParser = new MockConfigurationParser();

            defaultStartup.InitLogger(null);

            Configuration configuration = configurationParser.ParseConfiguration();
            IManager routingControllerManager = new RoutingControllerManager(configuration);
            routingControllerManager.Start();
        }
    }
}
