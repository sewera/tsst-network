using NLog;
using System;
using cn.Models;
using cn.Utils;
using NLog.Config;
using NLog.Fluent;
using NLog.Targets;

namespace cn
{
    class ClientNode
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            LoggingConfiguration config = new LoggingConfiguration();
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget
            {
                Name = "console",
                Layout = "[${time} | ${level:format=FirstCharacter} | ${logger}] ${message}"
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            LogManager.Configuration = config;

            IUserInterface userInterface = new UserInterface();
            Configuration configuration = new Configuration();
            ClientNodeManager cnManager = new ClientNodeManager(configuration, userInterface);

            cnManager.StartClientNode();
        }
    }
}
