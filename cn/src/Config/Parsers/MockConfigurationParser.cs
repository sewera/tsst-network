using System.Collections.Generic;
using NLog;

namespace cn.Config.Parsers
{
    internal class MockConfigurationParser : IConfigurationParser
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public Configuration ParseConfiguration()
        {
            return new Configuration.Builder()
                .SetCableCloudAddress("127.0.0.1")
                .SetCableCloudPort(3001)
                .SetClientPortAlias("TestClient")
                .AddMplsLabel("H1",101)
                .AddMplsLabel("H2",102)
                .Build();
        }
    }
}
