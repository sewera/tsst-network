using NLog;

namespace Cc.Cmd.Parsers
{
    public class MockCommandParser : ICommandParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public Command ParseCommand(string input)
        {
            LOG.Trace($"Command: {input}");
            LOG.Trace("Returning SEND command");
            return new Command(CommandType.SEND);
        }
    }
}
