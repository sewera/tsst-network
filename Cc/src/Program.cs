using NLog;
using NLog.Config;
using NLog.Targets;
using Cc.Config.Parsers;

namespace Cc
{
    class Program
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
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
            var cableCloud = new CableCloud(configurationParser);
        }
    }
}
