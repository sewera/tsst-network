using System;
using Common.Config.Parsers;
using Common.Models;
using Common.Startup;
using ConnectionController.Config;
using ConnectionController.Config.Parsers;

namespace ConnectionController
{
    public class ConnectionController
    {
        public static void Main(string[] args)
        {
            DefaultStartup<ConnectionController> defaultStartup = new DefaultStartup<ConnectionController>();
            defaultStartup.InitArgumentParse(args);
            IConfigurationParser<Configuration> configurationParser;

            if (defaultStartup.ChooseXmlParser())
                configurationParser = new XmlConfigurationParser(defaultStartup.Filename); 
            else
                configurationParser = new MockConfigurationParser();

            defaultStartup.InitLogger(null);

            Configuration configuration = configurationParser.ParseConfiguration();
            
            IConnectionControllerState connectionControllerState = configuration.ConnectionControllerType switch
            {
                "node" => new ConnectionControllerStateNode(configuration.ServerAddress,
                    configuration.CcPeerCoordinationRemotePorts,
                    configuration.CcConnectionRequestRemotePorts,
                    configuration.NnFibInsertRemotePort,
                    configuration.RcRouteTableQueryRemotePort), // Add things from config
                "domain" => new ConnectionControllerStateDomain(configuration.ServerAddress, configuration.CcPeerCoordinationRemotePorts), // Add things from config
                "subnetwork" => new ConnectionControllerStateSubnetwork(configuration.ServerAddress,
                    configuration.CcPeerCoordinationRemotePorts,
                    configuration.CcConnectionRequestRemotePorts,
                    configuration.LrmRemotePorts,
                    configuration.CcNames,
                    configuration.RcRouteTableQueryRemotePort),
                _ => throw new Exception("Not a known ConnectionController type")
            };
            IManager connectionControllerManager = new ConnectionControllerManager(configuration,
                connectionControllerState.OnConnectionRequest,
                connectionControllerState.OnPeerCoordination);
            connectionControllerManager.Start();
        }
    }
}
