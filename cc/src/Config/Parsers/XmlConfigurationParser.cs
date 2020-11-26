using System.Linq;
using System.Xml.Linq;
using NLog;

namespace cc.Config.Parsers
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

            configurationBuilder.SetListeningAddress(xelement.Descendants("cable_cloud_address").First().Value);
            configurationBuilder.SetListeningPort(short.Parse(xelement.Descendants("cable_cloud_port").First().Value));

            foreach (XElement element in xelement.Descendants("connection"))
            {
                string[] connection = element.Value.Split(',', 2);
                LOG.Trace($"{connection[0]} <---> {connection[1]}");
                    
                configurationBuilder.AddConnection((connection[0], connection[1], true));
            }
            
            return configurationBuilder.Build();
        }
    }
}
