using System.Xml;
using NLog;

namespace nn.Config.Parsers
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
            LOG.Debug($"Reading configuration from {_filename}");
            XmlTextReader reader = new XmlTextReader(_filename);

            Configuration.Builder configurationBuilder = new Configuration.Builder();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "cable-cloud-port":
                            configurationBuilder.SetCableCloudPort(reader.ReadElementContentAsInt());
                            break;
                        case "client-port-alias":
                            configurationBuilder.AddClientPortAlias(reader.ReadElementContentAsString());
                            break;
                    }
                }
            }
            return configurationBuilder.Build();
        }
    }
}
