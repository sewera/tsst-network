using System;
using ClientNode.Config;
using ClientNode.Config.Parsers;
using ClientNode.Ui;
using ClientNode.Ui.Parsers;
using Common.Config.Parsers;
using Common.Models;
using Common.Networking.Client.Persistent;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace ClientNode
{
    class ClientNode
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

            IConfigurationParser<Configuration> configurationParser;
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
                FileName = logs + "/ClientNode_" + configuration.ClientAlias + ".log",
                DeleteOldFileOnStartup = true,
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;

            ICommandParser commandParser = new CommandParser(configuration);
            IPersistentClientPort<MplsPacket> clientPort = new PersistentClientPort<MplsPacket>(configuration.ClientPortAlias,
                configuration.CableCloudAddress, configuration.CableCloudPort);
            IClientNodeManager clientNodeManager = new ClientNodeManager(configuration, clientPort);

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
