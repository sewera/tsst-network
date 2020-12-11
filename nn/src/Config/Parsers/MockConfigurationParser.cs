using NLog;

namespace nn.Config.Parsers
{
    internal class MockConfigurationParser : IConfigurationParser
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
                .Build();
        }
    }
}
