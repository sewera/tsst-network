using NLog;
using NLog.Config;
using NLog.Targets;
using cc.Config;
using cc.Config.Parsers;

namespace cc
{
    class Program
    {
        private static Logger LOG = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}",
            };
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget, "*");
            LogManager.Configuration = config;

            LOG.Debug("Debug message");
            LOG.Info("Info message");
            LOG.Warn("Warn message");
            LOG.Error("Error message");
            LOG.Fatal("Fatal message");

            IConfigurationParser configurationParser = new MockConfigurationParser("");
            CableCloud cableCloud = new CableCloud(configurationParser);
        }
    }
}
