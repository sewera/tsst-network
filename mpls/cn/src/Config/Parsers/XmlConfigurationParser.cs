using System.Linq;
using System.Xml.Linq;
using NLog;

namespace cn.Config.Parsers 
{
    internal class XmlConfigurationParser : IConfigurationParser
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

            configurationBuilder.SetCableCloudAddress(xelement.Descendants("cable_cloud_address").First().Value);
            configurationBuilder.SetCableCloudPort(int.Parse(xelement.Descendants("cable_cloud_port").First().Value));
			configurationBuilder.SetClientAlias(xelement.Descendants("client_alias").First().Value);
            configurationBuilder.SetClientPortAlias(xelement.Descendants("client_port").First().Value);

            foreach (XElement element in xelement.Descendants("label"))
            {
                LOG.Trace($"Remote client node: {element.FirstAttribute.Value}\tLabel: {element.Value}");
                configurationBuilder.AddMplsLabel(element.FirstAttribute.Value, long.Parse(element.Value));
            }
            
            return configurationBuilder.Build();
        }
    }
}
