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
            Configuration configuration = new Configuration("resources/configuration.xml");
            configuration.ReadConfigFile();
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
