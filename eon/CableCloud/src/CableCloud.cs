using System;
using CableCloud.Config;
using CableCloud.Config.Parsers;
using CableCloud.Networking.Client;
using CableCloud.Networking.Forwarding;
using CableCloud.Networking.Listeners;
using CableCloud.Ui;
using CableCloud.Ui.Parsers;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace CableCloud
{
    public class CableCloud
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            string filename = "";
            string logs = "";
            try
            {
                LOG.Trace($"Args: {string.Join(", ", args)}");
                if (args[0] == "-c")
                    filename = args[1];
                if (args[2] == "-l")
                    logs = args[3];
                else
                    LOG.Warn("Use '-c <filename> -l <log_filename>' to pass a config file to program and set where logs should be");
            }
            catch (IndexOutOfRangeException)
            {
                LOG.Warn("Use '-c <filename> -l <log_filename>' to pass a config file to program and set where logs should be");
                LOG.Warn("Using MockConfigurationParser instead");
            }

            IConfigurationParser configurationParser;
            if (string.IsNullOrWhiteSpace(filename))
                configurationParser = new MockConfigurationParser();
            else
                configurationParser = new XmlConfigurationParser(filename);

            Configuration configuration = configurationParser.ParseConfiguration();
            LoggingConfiguration config = new LoggingConfiguration();
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            FileTarget fileTarget = new FileTarget
            {
                FileName = logs + "/CableCloud.log",
                DeleteOldFileOnStartup = true,
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;

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
