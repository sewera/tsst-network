using cn.Config;
using cn.Config.Parsers;
using cn.Networking;
using cn.Ui;
using cn.Ui.Parsers;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace cn
{
    class ClientNode
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
            IClientNodeManager clientNodeManager = new ClientNodeManager(configuration, clientPortFactory);

            IUserInterface userInterface = new UserInterface(commandParser, clientNodeManager);

            userInterface.Start();
        }
    }
}
