using System;
using System.Threading;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace ms
{
    class ManagementSystem
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            LoggerSetup();

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

            if(string.IsNullOrEmpty(filename))
                filename = "resources/ManagementSystem.xml";

            Configuration configuration = new Configuration(filename);
            configuration.ReadConfigFile();

            try
            {
                Console.Title = "MS";
            }
            catch (Exception)
            {
                LOG.Trace("Could not set the title");
            }

            IManagementManager mm = new ManagementManager();
            mm.ReadConfig(configuration);
            mm.startListening();
            MessageSender.ReadConfig(configuration);
            new Thread(MessageSender.Start).Start();
            UserInterface.Start();
        }

        private static void LoggerSetup()
        {
            LoggingConfiguration config = new LoggingConfiguration();
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            LogManager.Configuration = config;
        }
    }
}
