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
                configurationParser = new MockConfigurationParser(); // TODO: Change for XmlConfigurationParser
            else
                configurationParser = new MockConfigurationParser();

            defaultStartup.InitLogger(null);

            Configuration configuration = configurationParser.ParseConfiguration();
            IManager connectionControllerManager = new ConnectionControllerManager(configuration,
                packet => new GenericDataPacket.Builder().SetType(GenericPacket.PacketType.Response).SetData(packet.Data).Build(),
                packet => new GenericDataPacket.Builder().SetType(GenericPacket.PacketType.Response).SetData(packet.Data).Build());
            // TODO: Those are only mock delegates
            connectionControllerManager.Start();
        }
    }
}
