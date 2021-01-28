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
                configurationParser = new XmlConfigurationParser(defaultStartup.Filename); // TODO: Change for XmlConfigurationParser
            else
                configurationParser = new MockConfigurationParser();

            defaultStartup.InitLogger(null); // TODO: Set log suffix from configuration

            Configuration configuration = configurationParser.ParseConfiguration();
            IManager networkCallControllerManager = new NetworkCallControllerManager(configuration,
                packet => new GenericDataPacket.Builder().SetType(GenericPacket.PacketType.Response).SetData(packet.Data).Build(),
                packet => new GenericDataPacket.Builder().SetType(GenericPacket.PacketType.Response).SetData(packet.Data).Build(),
                packet => new GenericDataPacket.Builder().SetType(GenericPacket.PacketType.Response).SetData(packet.Data).Build());
            // TODO: Those are only mock delegates, make proper ones

            networkCallControllerManager.Start();
        }
    }
}
