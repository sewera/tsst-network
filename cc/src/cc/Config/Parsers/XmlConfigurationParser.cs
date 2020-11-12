using NLog;

namespace cc.Config.Parsers
{
    class XmlConfigurationParser : IConfigurationParser
    {
        private static Logger LOG = LogManager.GetCurrentClassLogger();

        private string filename;

        public XmlConfigurationParser(string filename)
        {
            this.filename = filename;
        }

        public Configuration ParseConfiguration()
        {
            return null; // TODO
        }
    }
}
