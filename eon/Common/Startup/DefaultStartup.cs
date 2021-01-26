using System;
using Common.Config.Parsers;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Common.Startup
{
    public class DefaultStartup<TClass>
    {
        public string Filename { get; set; }
        public string LogsDirectory { get; set; }
        private static readonly Logger LOG = LogManager.GetLogger(typeof(TClass).Name);

        public void InitArgumentParse(string[] args)
        {
            Filename = "";
            LogsDirectory = "";
            try
            {
                LOG.Trace($"Args: {string.Join(", ", args)}");
                if (args[0] == "-c")
                    Filename = args[1];
                if (args[2] == "-l")
                    LogsDirectory = args[3];
                else
                    LOG.Warn("Use '-c <filename> -l <log_directory>' to pass a config file to program and set where logs should be");
            }
            catch (IndexOutOfRangeException)
            {
                LOG.Warn("Use '-c <filename> -l <log_directory>' to pass a config file to program and set where logs should be");
                LOG.Warn("Using MockConfigurationParser instead");
            }
        }

        public void InitLogger(string logFilenameSuffix)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            string suffix = string.IsNullOrEmpty(logFilenameSuffix) ? "" : $"_{logFilenameSuffix}";
            FileTarget fileTarget = new FileTarget
            {
                FileName = $"{LogsDirectory}/{typeof(TClass).Name}{suffix}.log",
                DeleteOldFileOnStartup = true,
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;

        }

        public bool ChooseXmlParser()
        {
            return !string.IsNullOrWhiteSpace(Filename);
        }

        public void SetTitle(string title)
        {
            try
            {
                Console.Title = title;
            }
            catch (Exception)
            {
                LOG.Trace("Could not set the title");
            }
        }
    }
}
