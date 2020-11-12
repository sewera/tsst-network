using cc.Config;
using cc.Config.Parsers;

namespace cc
{
    class CableCloud
    {
        private IConfigurationParser configurationParser;
        private Configuration configuration;

        public CableCloud(IConfigurationParser configurationParser)
        {
            this.configurationParser = configurationParser;
            this.configuration = configurationParser.ParseConfiguration();
        }
    }
}
