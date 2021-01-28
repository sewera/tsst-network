using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Common.Config.Parsers;
using NetworkCallController.Config;
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

            configurationBuilder.SetServerAddress(IPAddress.Parse((xelement.Descendants("server_address").First().Value)));
            configurationBuilder.SetConnectionRequestLocalPort(int.Parse(xelement.Descendants("ncc_connection_request_local_port").First().Value));
            configurationBuilder.SetCallCoordinationLocalPort(int.Parse(xelement.Descendants("ncc_call_coordination_local_port").First().Value));
            configurationBuilder.SetCallTeardownLocalPort(int.Parse(xelement.Descendants("ncc_call_teardown_local_port").First().Value));
            configurationBuilder.SetConnectionRequestRemotePort(int.Parse(xelement.Descendants("cc_connection_request_remote_port").First().Value));
            configurationBuilder.SetCallCoordinationRemotePort(int.Parse(xelement.Descendants("ncc_call_coordination_remote_port").First().Value));
            configurationBuilder.SetDomain(xelement.Descendants("domain").First().Value);

            foreach (XElement element in xelement.Descendants("client_port"))
            {
                LOG.Trace($"NCC: ClientName: {element.FirstAttribute.Value} ClientPortAlias: {element.Value}");
                Console.WriteLine($"{element.FirstAttribute.Value} {element.Value}");
                configurationBuilder.AddClientPortAlias(element.FirstAttribute.Value, element.Value);
            }
            
            foreach (XElement element in xelement.Descendants("port_domain"))
            {
                LOG.Trace($"NCC: PortTemplate: {element.FirstAttribute.Value} Domain: {element.Value}");
                Console.WriteLine($"{element.FirstAttribute.Value} {element.Value}");
                configurationBuilder.AddPortDomain(element.FirstAttribute.Value, element.Value);
            }
            
            return configurationBuilder.Build();
        }
    }
}
