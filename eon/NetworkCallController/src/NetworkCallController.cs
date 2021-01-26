using System;
using Common.Config.Parsers;
using Common.Models;
using Common.Networking.Server.OneShot;
using Common.Startup;
using NetworkCallController.Config;
using NetworkCallController.Config.Parsers;

namespace NetworkCallController
{
    class NetworkCallController
    {
        static void Main(string[] args)
        {
            DefaultStartup<NetworkCallController> defaultStartup = new DefaultStartup<NetworkCallController>();
            defaultStartup.InitArgumentParse(args);
            IConfigurationParser<Configuration> configurationParser;

            if (defaultStartup.ChooseXmlParser())
                configurationParser = new MockConfigurationParser(); // TODO: Change for XmlConfigurationParser
            else
                configurationParser = new MockConfigurationParser();

            defaultStartup.InitLogger(null); // TODO: Set log suffix from configuration

            Configuration configuration = configurationParser.ParseConfiguration();
            IManager networkCallControllerManager = new NetworkCallControllerManager(configuration);
            networkCallControllerManager.Start();
        }
    }
}
