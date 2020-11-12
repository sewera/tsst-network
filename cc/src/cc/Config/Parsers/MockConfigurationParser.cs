using NLog;

namespace cc.Config.Parsers
{
    class MockConfigurationParser : IConfigurationParser
    {
        private static Logger LOG = LogManager.GetCurrentClassLogger();

        private string filename;

        public MockConfigurationParser(string filename)
        {
            this.filename = filename;
        }

        public Configuration ParseConfiguration()
        {
            const string listeningPort = "3001";
            LOG.Debug(string.Format("Simulating Configuration parsing with default port: {0}", listeningPort));
            return new Configuration.Builder().SetListeningPort("3001").Build();
        }
    }
}
