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
                .SetRouterAlias("R1")
                .AddPortAlias("11")
                .AddPortAlias("12")
                .AddPortAlias("13")
                .AddPortAlias("14")
                .AddLrm(("11", new Configuration.LrmConfiguration.LrmBuilder()
                    .SetRemotePortAlias("102")
                    .SetLrmLinkConnectionRequestLocalPort(41001)
                    .SetLrmLinkConnectionRequestRemotePort(41002)
                    .SetRcLocalTopologyRemotePort(42001)
                    .Build()))
                .AddLrm(("12", new Configuration.LrmConfiguration.LrmBuilder()
                    .SetRemotePortAlias("103")
                    .SetLrmLinkConnectionRequestLocalPort(41001)
                    .SetLrmLinkConnectionRequestRemotePort(41002)
                    .SetRcLocalTopologyRemotePort(42001)
                    .Build()))
                .AddLrm(("13", new Configuration.LrmConfiguration.LrmBuilder()
                    .SetRemotePortAlias("104")
                    .SetLrmLinkConnectionRequestLocalPort(41001)
                    .SetLrmLinkConnectionRequestLocalPort(41001)
                    .SetLrmLinkConnectionRequestRemotePort(41002)
                    .SetRcLocalTopologyRemotePort(42001)
                    .Build()))
                .AddLrm(("14", new Configuration.LrmConfiguration.LrmBuilder()
                    .SetRemotePortAlias("105")
                    .SetLrmLinkConnectionRequestLocalPort(41001)
                    .SetLrmLinkConnectionRequestLocalPort(41001)
                    .SetLrmLinkConnectionRequestRemotePort(41002)
                    .SetRcLocalTopologyRemotePort(42001)
                    .Build()))
                .Build();
        }
    }
}
