using Common.Config.Parsers;
using NLog;

namespace NetworkNode.Config.Parsers
{
    internal class MockConfigurationParser : IConfigurationParser<Configuration>
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public Configuration ParseConfiguration()
        {
            return new Configuration.Builder()
                .SetCableCloudAddress("127.0.0.1")
                .SetCableCloudPort(3001)
                .SetManagementSystemAddress("127.0.0.1")
                .SetRouterAlias("R1")
                .SetManagementSystemPort(4001)
                .AddPortAlias("11")
                .AddPortAlias("12")
                .AddPortAlias("13")
                .AddPortAlias("14")
                .Build();
        }
    }
}
