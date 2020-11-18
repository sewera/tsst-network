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
            Config config = new Config();
            config.ReadConfigFile("ManagementSystem.xml");
            IManagementManager mm = new ManagementManager();
            mm.ReadConfig(config);
            mm.startListening();
            MessageSender.ReadConfig(config);
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
