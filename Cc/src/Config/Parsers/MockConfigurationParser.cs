using NLog;

namespace Cc.Config.Parsers
{
    internal class MockConfigurationParser : IConfigurationParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public Configuration ParseConfiguration()
        {
            const short listeningPort = 3001;
            LOG.Debug($"Simulating Configuration parsing with default port: {listeningPort}");
            return new Configuration.Builder().SetListeningPort(listeningPort).Build();
        }
    }
}
