using cc.Config;
using cc.Config.Parsers;
using cc.Networking.Client;
using cc.Networking.Forwarding;
using cc.Networking.Listeners;
using cc.Ui;
using cc.Ui.Parsers;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace cc
{
    internal class CableCloud
    {
        public static void Main(string[] args)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            LogManager.Configuration = config;

            //IConfigurationParser configurationParser = new MockConfigurationParser();
            IConfigurationParser configurationParser = new XmlConfigurationParser("resources/CableCloud.xml");
            Configuration configuration = configurationParser.ParseConfiguration();
            
            IClientWorkerFactory clientWorkerFactory = new ClientWorkerFactory();
            IListener listener = new Listener(configuration, clientWorkerFactory);
            IPacketForwarder packetForwarder = new PacketForwarder(configuration);

            ICableCloudManager cableCloudManager = new CableCloudManager(configuration, listener, packetForwarder);

            ICommandParser commandParser = new CommandParser(configuration);
            IUserInterface userInterface = new UserInterface(commandParser, cableCloudManager);

            userInterface.Start();
        }
    }
}
