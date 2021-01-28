using System.Linq;
using System.Net;
using System.Xml.Linq;
using Common.Config.Parsers;
using NLog;

namespace ConnectionController.Config.Parsers
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

            configurationBuilder.SetConnectionControllerType(xelement.Descendants("cc_type").First().Value);
            configurationBuilder.SetConnectionRequestLocalPort(
                int.Parse(xelement.Descendants("cc_connection_request_listener_local_port").First().Value));
            configurationBuilder.SetPeerCoordinationLocalPort(
                int.Parse(xelement.Descendants("cc_peer_coordination_listener_local_port").First().Value));
            configurationBuilder.SetServerAddress(IPAddress.Parse(xelement.Descendants("server_address").First().Value));
            
            foreach (XElement element in xelement.Descendants("cc_name"))
            {
                LOG.Trace($"CC: PortPattern: {element.FirstAttribute.Value} CcName: {element.Value}");
                configurationBuilder.AddCcName(element.FirstAttribute.Value, element.Value);
            }

           switch (xelement.Descendants("cc_type").First().Value)
           {
               case "node":
                   foreach (XElement element in xelement.Descendants("cc_peer_coordination_remote_port"))
                   {
                       LOG.Trace($"CC: CcName: {element.FirstAttribute.Value} CcPeerCoordinationRemotePort: {element.Value}");
                       configurationBuilder.AddCcPeerCoordinationRemotePort(element.FirstAttribute.Value, int.Parse(element.Value));
                   }

                   break;
               
               case "domain":
                   configurationBuilder.SetPeerCoordinationRemotePort(
                       int.Parse(xelement.Descendants("cc_peer_coordination_remote_port").First().Value));

                   foreach (XElement element in xelement.Descendants("cc_connection_request_remote_port"))
                   {
                       LOG.Trace($"CC: CcName: {element.FirstAttribute.Value} CcConnectionRequestRemotePort: {element.Value}");
                       configurationBuilder.AddCcConnectionRequestRemotePort(element.FirstAttribute.Value, int.Parse(element.Value));
                   }

                   break;
               
               case "subnetwork":
                   foreach (XElement element in xelement.Descendants("cc_connection_request_remote_port"))
                   {
                       LOG.Trace($"CC: CcName: {element.FirstAttribute.Value} CcConnectionRequestRemotePort: {element.Value}");
                       configurationBuilder.AddCcConnectionRequestRemotePort(element.FirstAttribute.Value, int.Parse(element.Value));
                   }  
               
                   foreach (XElement element in xelement.Descendants("cc_peer_coordination_remote_port"))
                   {
                       LOG.Trace($"CC: CcName: {element.FirstAttribute.Value} CcPeerCoordinationRemotePort: {element.Value}");
                       configurationBuilder.AddCcPeerCoordinationRemotePort(element.FirstAttribute.Value, int.Parse(element.Value));
                   }

                   break;
           }
            
            return configurationBuilder.Build();
        }
    }
}
