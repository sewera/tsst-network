using System;
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

            string filename = "";
            try
            {
                LOG.Trace($"Args: {string.Join(", ", args)}");
                if (args[0] == "-c")
                    filename = args[1];
                else if (args[1] == "-c")
                    filename = args[2];
                else
                    LOG.Warn("Use '-c <filename>' to pass a config file to program");
            }
            catch (IndexOutOfRangeException)
            {
                LOG.Warn("Use '-c <filename>' to pass a config file to program");
                LOG.Warn("Using MockConfigurationParser instead");
            }

            IConfigurationParser configurationParser;
            if (string.IsNullOrWhiteSpace(filename))
                configurationParser = new MockConfigurationParser();
            else
                configurationParser = new XmlConfigurationParser(filename);

            Configuration configuration = configurationParser.ParseConfiguration();

            IClientPortFactory clientPortFactory = new ClientPortFactory(configuration);
            ICommandParser commandParser = new CommandParser(configuration);
            IClientNodeManager clientNodeManager = new ClientNodeManager(configuration, clientPortFactory);

            IUserInterface userInterface = new UserInterface(commandParser, clientNodeManager);

            try
            {
                Console.Title = configuration.ClientAlias;
            }
            catch (Exception)
            {
                LOG.Trace("Could not set the title");
            }

            userInterface.Start();
        }
    }
}
