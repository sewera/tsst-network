using Cc.Config;
using Cc.Config.Parsers;
using Cc.Networking.Controllers;
using Cc.Networking.Forwarders;
using Cc.Networking.Listeners;
using Cc.Networking.Receivers;
using Cc.Networking.Tables;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Cc
{
    internal class Program
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            LogManager.Configuration = config;

            IConfigurationParser configurationParser = new MockConfigurationParser();
            var configuration = configurationParser.ParseConfiguration();
            
            IDataReceiverFactory dataReceiverFactory = new RawDataReceiverFactory();
            IConnectionTable connectionTable = new CcConnectionTable();
            IClientController clientController = new ClientController(dataReceiverFactory);
            IListener listener = new Listener(clientController, configuration);
            IPacketForwarder packetForwarder = new CcPacketForwarder(connectionTable);

            var cableCloud = new CableCloud.Builder()
                .SetConfiguration(configuration)
                .SetListener(listener)
                .SetClientController(clientController)
                .SetPacketForwarder(packetForwarder)
                .SetDataReceiverFactory(dataReceiverFactory)
                .Build();
            
            cableCloud.Start();
        }
    }
}
