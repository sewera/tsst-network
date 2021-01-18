using System.Linq;
using System.Xml.Linq;
using ms.Models;
using NLog;

namespace ms.Config.Parsers
{
    public class XmlConfigurationParser : IConfigurationParser
    {
        private readonly string _filename;
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public XmlConfigurationParser(string filename)
        {
            _filename = filename;
        }

        public Configuration ParseConfiguration()
        {
            Configuration.Builder configurationBuilder = new Configuration.Builder();

            LOG.Debug($"Reading configuration from {_filename}");
            XElement xElement = XElement.Load(_filename);

            configurationBuilder.SetPort(int.Parse(xElement.Descendants("listener_port").First().Value));

            foreach (XElement element in xElement.Descendants("message"))
            {
                LOG.Trace($"Router: {element.FirstAttribute.Value}\tConfig message: {element.Value}");
                configurationBuilder.AddConfigMessage(new Message(element.FirstAttribute.Value,element.Value));
            }

            return configurationBuilder.Build();
        }
    }
}
