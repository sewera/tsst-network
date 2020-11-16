using NLog;

namespace Cc.Cmd.Parsers
{
    public class MockCommandParser : ICommandParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public Command ParseCommand(string input)
        {
            LOG.Debug($"Command: {input}");
            LOG.Debug("Returning SEND command");
            return new Command(CommandType.SEND);
        }
    }
}
