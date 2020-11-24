using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
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

            configurationBuilder.SetCableCloudAddress("127.0.0.1");
            configurationBuilder.SetCableCloudPort(Int32.Parse(xelement.Descendants("cable_cloud_port").First().Value));
            configurationBuilder.SetClientPortAlias(xelement.Descendants("client_port").First().Value);

            foreach (XElement element in xelement.Descendants("label"))
            {
                configurationBuilder.AddMplsLabel(Int32.Parse(element.Value));
            }
            
            return configurationBuilder.Build();
        }
    }
}
