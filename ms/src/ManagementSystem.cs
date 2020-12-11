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
            }

            LoggerSetup(logs);

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
            new Thread(MessageSender.SendConfigCommands).Start();
            UserInterface.Start();
        }

        private static void LoggerSetup(string logs)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            FileTarget fileTarget = new FileTarget
            {
                FileName = logs + "/ManagementSystem.log",
                DeleteOldFileOnStartup = true,
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;
        }
    }
}
