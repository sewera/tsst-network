using System;
using Common.Config.Parsers;
using Common.Models;
using Common.Networking.Client.Persistent;
using NLog;
using NLog.Config;
using NLog.Targets;
using NetworkNode.Config;
using NetworkNode.Config.Parsers;
using NetworkNode.Networking.Forwarding;

namespace NetworkNode
{
    class NetworkNode
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
                FileName = logs + "/NetworkNode_" + configuration.RouterAlias + ".log",
                DeleteOldFileOnStartup = true,
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;

            IPacketForwarder packetForwarder = new MplsPacketForwarder();
            //IPacketForwarder packetForwarder = new MockPacketForwarder(configuration);
            IPersistentClientPortFactory<MplsPacket> clientPortFactory = new PersistentClientPortFactory<MplsPacket>(configuration.CableCloudAddress, configuration.CableCloudPort);
            INetworkNodeManager networkNodeManager = new NetworkNodeManager(configuration, packetForwarder, clientPortFactory);

            try
            {
                Console.Title = configuration.RouterAlias;
            }
            catch (Exception)
            {
                LOG.Trace("Could not set the title");
            }

            networkNodeManager.Start();
        }
    }
}
