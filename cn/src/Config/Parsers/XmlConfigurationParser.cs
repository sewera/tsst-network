using System.Linq;
using System.Xml.Linq;
using NLog;
using NLog.Fluent;

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

            //foreach (XElement element in xelement.Descendants("labels"))
            //{
            //    LOG.Trace("Host: {}/nLabel: {}",element.Element("label").Value, long.Parse(element.Value));
            //    configurationBuilder.AddMplsLabel(element.Element("label").Value, long.Parse(element.Value));
            //}
            // TODO: FINISH
            var labels = from c in xelement.Descendants("labels")
                select c.Element("host");

            foreach (string host in labels)
            {
                LOG.Trace(host);
            }
            
            return configurationBuilder.Build();
        }
    }
}
