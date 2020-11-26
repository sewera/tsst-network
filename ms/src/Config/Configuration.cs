using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NLog;

namespace ms
{
    public class Configuration
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly string _filename;
        
        /// <summary>
        /// Listener socket port
        /// </summary>
        public short Port { get; set; }
        
        /// <summary>
        /// List of messages to be sent to network nodes in order to config their MPLS-tables
        /// </summary>
        public List<Message> configMessages = new List<Message>();

        public Configuration(string filename)
        {
            _filename = filename;
        }
        
        public Configuration ReadConfigFile()
        {
            Configuration result = new Configuration(_filename);

            LOG.Debug($"Reading configuration from {_filename}");
            XElement xelement = XElement.Load(_filename);	

            Port = short.Parse(xelement.Descendants("listener_port").First().Value);

            foreach (XElement element in xelement.Descendants("message"))
            {
                LOG.Trace($"Router: {element.FirstAttribute.Value}\tConfig message: {element.Value}");
                configMessages.Add(new Message(element.FirstAttribute.Value,element.Value));
            }

            return result;
        }
    }
}
