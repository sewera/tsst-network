using System.Linq;
using System.Xml.Linq;
using Common.Config.Parsers;
using NLog;

namespace RoutingController.Config.Parsers
{
    internal class XmlConfigurationParser : IConfigurationParser<Configuration>
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly string _filename;

        public XmlConfigurationParser(string filename)
        {
            _filename = filename;
        }

        public Configuration ParseConfiguration()
        {
            Configuration.Builder configurationBuilder = new Configuration.Builder();

            LOG.Trace($"Reading configuration from {_filename}");
            XElement xelement = XElement.Load(_filename);

            configurationBuilder.SetRouteTableQueryLocalPort(
                int.Parse(xelement.Descendants("rc_route_table_query_local_port").First().Value));
            configurationBuilder.SetLocalTopologyLocalPort(
                int.Parse(xelement.Descendants("rc_local_topology_local_port").First().Value));
            configurationBuilder.SetNetworkTopologyLocalPort(
                int.Parse(xelement.Descendants("rc_network_topology_local_port").First().Value));

            foreach (XElement element in xelement.Descendants("row"))
            {
                Configuration.RouteTableRow.RouteTableRowBuilder routeTableRowBuilder =
                    new Configuration.RouteTableRow.RouteTableRowBuilder();
                routeTableRowBuilder.SetSrc(element.Descendants("src").First().Value);
                routeTableRowBuilder.SetDst(element.Descendants("dst").First().Value);
                routeTableRowBuilder.SetGateway(element.Descendants("gateway").First().Value);

                LOG.Trace(
                    $"src: {element.Descendants("src").First().Value} " +
                    $"dst: {element.Descendants("dst").First().Value} " +
                    $"gateway: {element.Descendants("gateway").First().Value}");
                configurationBuilder.AddRouteTableRow(routeTableRowBuilder.Build());
            }
            
            return configurationBuilder.Build();
        }
    }
}
