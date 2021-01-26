using Common.Config.Parsers;

namespace RoutingController.Config.Parsers
{
    public class MockConfigurationParser : IConfigurationParser<Configuration>
    {
        public Configuration ParseConfiguration()
        {
            return new Configuration.Builder()
                .SetLocalTopologyLocalPort(6011)
                .SetNetworkTopologyLocalPort(6012)
                .SetRouteTableQueryLocalPort(6013)
                .Build();
        }
    }
}
