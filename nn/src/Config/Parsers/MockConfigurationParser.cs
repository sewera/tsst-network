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
                .SetManagementSystemPort(4001)
                .AddPortAlias("R1/1")
                .AddPortAlias("R1/2")
                .AddPortAlias("R1/3")
                .AddMplsLabel(100)
                .AddMplsLabel(200)
                .Build();
        }
    }
}
