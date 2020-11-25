using NLog;

namespace cc.Config.Parsers
{
    internal class XmlConfigurationParser : IConfigurationParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly string _filename;

        public XmlConfigurationParser(string filename)
        {
            _filename = filename;
        }

        public Configuration ParseConfiguration()
        {
            LOG.Debug($"Using file: {_filename}");
            return null; // TODO
        }
    }
}
