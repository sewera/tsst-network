using NLog;

namespace nn.src.Config.Parsers
{
    internal class MockConfigurationParser : IConfigurationParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public Configuration ParseConfiguration()
        {
            return new Configuration.Builder()
                .SetCableCloudAddress("127.0.0.1")
                .SetCableCloudPort(3001)
                .AddClientPortAlias("R1/1")
                .AddClientPortAlias("R1/2")
                .AddClientPortAlias("R1/3")
                .AddMplsLabel(100)
                .AddMplsLabel(200)
                .Build();
        }
    }
}
