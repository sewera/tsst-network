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
            
            LOG.Debug($"Reading configuration from {_filename}");
            XElement xelement = XElement.Load(_filename);

            configurationBuilder.SetRouterAlias(xelement.Descendants("router_alias").First().Value);
            configurationBuilder.SetCableCloudAddress(xelement.Descendants("cable_cloud_address").First().Value);
            configurationBuilder.SetCableCloudPort(int.Parse(xelement.Descendants("cable_cloud_port").First().Value));
            configurationBuilder.SetManagementSystemAddress(xelement.Descendants("management_system_address").First().Value);
            configurationBuilder.SetManagementSystemPort(int.Parse(xelement.Descendants("management_system_port").First().Value));

            foreach (XElement element in xelement.Descendants("port_alias"))
            {
                LOG.Trace($"Router {xelement.Descendants("router_alias").First().Value} port alias: {element.Value}");
                configurationBuilder.AddPortAlias(element.Value);
            }
            
            return configurationBuilder.Build();
        }
    }
}
