using NLog;
using NLog.Config;
using NLog.Targets;
using nn.src.Config;
using nn.src.Config.Parsers;
using nn.src.Networking;
using nn.src.Ui;
using nn.src.Ui.Parsers;

namespace nn.src
{
    class NetworkNode
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

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

            // IConfigurationParser configurationParser = new XmlConfigurationParser("resources/configuration.xml");
            IConfigurationParser configurationParser = new MockConfigurationParser();
            Configuration configuration = configurationParser.ParseConfiguration();

            IClientPortFactory clientPortFactory = new ClientPortFactory(configuration);
            ICommandParser commandParser = new CommandParser();
            INetworkNodeManager networkNodeManager = new NetworkNodeManager(configuration, clientPortFactory);

            IUserInterface userInterface = new UserInterface(commandParser, networkNodeManager);

            userInterface.Start();
        }
    }
}
