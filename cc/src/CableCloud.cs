using System;
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
    public class CableCloud
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
            
            IClientWorkerFactory clientWorkerFactory = new ClientWorkerFactory();
            IListener listener = new Listener(configuration, clientWorkerFactory);
            IPacketForwarder packetForwarder = new PacketForwarder(configuration);

            ICableCloudManager cableCloudManager = new CableCloudManager(configuration, listener, packetForwarder);

            ICommandParser commandParser = new CommandParser(configuration);
            IUserInterface userInterface = new UserInterface(commandParser, cableCloudManager);

            try
            {
                Console.Title = "CC";
            }
            catch (Exception)
            {
                LOG.Trace("Could not set the title");
            }

            userInterface.Start();
        }
    }
}
