using System.Net;
using System.Xml;

namespace cn.Utils
{
    public class Configuration : IConfiguration
    {
        /// <summary>Name of our host client node</summary>
        public string CnName { get; set; }

        /// <summary>IP address of our network elements, 
        /// for sake of this project - local host address</summary>
        public IPAddress IpAddress { get; set; }

        /// <summary>Port no of host client node</summary>
        public short CnPort { get; set; }

        /// <summary>Port no of cable cloud</summary>
        public short CloudPort { get; set; }

        public Configuration()
        {

        }

        public Configuration ReadConfigFile(string configFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new XmlTextReader(configFile));
            XmlNodeList xnl = doc.SelectNodes(string.Format("/hosts/host"));

            //TODO: finish
            return null;
        }
    }
}
