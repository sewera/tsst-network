using NLog;

namespace Cc.Config.Parsers
{
    internal class XmlConfigurationParser : IConfigurationParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

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
