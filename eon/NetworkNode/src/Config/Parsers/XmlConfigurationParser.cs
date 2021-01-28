using System.Linq;
using System.Xml.Linq;
using Common.Config.Parsers;
using NLog;

namespace NetworkNode.Config.Parsers
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

            configurationBuilder.SetRouterAlias(xelement.Descendants("router_alias").First().Value);
            configurationBuilder.SetCableCloudAddress(xelement.Descendants("cable_cloud_address").First().Value);
            configurationBuilder.SetCableCloudPort(int.Parse(xelement.Descendants("cable_cloud_port").First().Value));

            foreach (XElement element in xelement.Descendants("port_alias"))
            {
                LOG.Trace($"Router {xelement.Descendants("router_alias").First().Value} port alias: {element.Value}");
                configurationBuilder.AddPortAlias(element.Value);
            }
            
            foreach (XElement element in xelement.Descendants("lrm"))
            {
                Configuration.LrmConfiguration.LrmBuilder configurationLrmBuilder =
                    new Configuration.LrmConfiguration.LrmBuilder();
                configurationLrmBuilder.SetRemotePortAlias(element.Descendants("remote_port").First().Value);
                configurationLrmBuilder.SetLrmLinkConnectionRequestLocalPort(int.Parse(element
                    .Descendants("lrm_link_connection_request_local_port").First().Value));
                configurationLrmBuilder.SetLrmLinkConnectionRequestRemotePort(int.Parse(element
                    .Descendants("lrm_link_connection_request_remote_port").First().Value));
                configurationLrmBuilder.SetRcLocalTopologyRemotePort(int.Parse(element
                    .Descendants("rc_local_topology_remote_port").First().Value));
                
                LOG.Trace($"LRM {element.Descendants("local_port").First().Value}");
                configurationBuilder.AddLrm(element.Descendants("local_port").First().Value, configurationLrmBuilder.Build());
            }
            
            return configurationBuilder.Build();
        }
    }
}
