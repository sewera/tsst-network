using Cc.Config;
using Cc.Config.Parsers;

namespace Cc
{
    internal class CableCloud
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
