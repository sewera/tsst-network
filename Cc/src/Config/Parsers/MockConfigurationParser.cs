using NLog;

namespace Cc.Config.Parsers
{
    internal class MockConfigurationParser : IConfigurationParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private string filename;

        public MockConfigurationParser(string filename)
        {
            this.filename = filename;
        }

        public Configuration ParseConfiguration()
        {
            const string listeningPort = "3001";
            LOG.Debug($"Simulating Configuration parsing with default port: {listeningPort}");
            return new Configuration.Builder().SetListeningPort(3001).Build();
        }
    }
}
