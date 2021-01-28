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
                .AddRouteTableRow(new Configuration.RouteTableRow.RouteTableRowBuilder()
                    .SetSrc("1xx")
                    .SetDst("122")
                    .SetGateway("115")
                    .Build())
                .Build();
        }
    }
}
