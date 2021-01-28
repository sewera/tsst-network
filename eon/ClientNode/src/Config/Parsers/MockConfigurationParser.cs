using Common.Config.Parsers;
using NLog;

namespace ClientNode.Config.Parsers
{
    internal class MockConfigurationParser : IConfigurationParser<Configuration>
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public Configuration ParseConfiguration()
        {
            return new Configuration.Builder()
                .SetClientAlias("H0_Test")
                .SetCableCloudAddress("127.0.0.1")
                .SetCableCloudPort(3001)
                .SetNccConnectionRequestRemotePort(11811)
                .SetNccCallTeardownRemotePort(11813)
                .SetClientPortAlias("TestClient")
                .AddMplsLabel("H1",101)
                .AddMplsLabel("H2",102)
                .Build();
        }
    }
}
