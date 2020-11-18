using System.Collections.Generic;
using System.Xml;

namespace cn.Utils
{
    public class Configuration
    {
       
        /// <summary>
        /// Alias representation of cable cloud port
        /// </summary>
        public string CableCloudPort { get; set; }

        /// <summary>
        /// Alias representation host client node port
        /// </summary>
        public string SourcePort { get; set; }
        
        /// <summary>
        /// List of MPLS labels
        /// </summary>
        public IList<short> Labels { get; set; }

        public Configuration()
        {

        }

        /// <summary>Read configuration file and 
        /// load set of data about network elements</summary>
        /// <param name="configFile">File with network data
        /// saved in defined convention</param>
        /// <returns>Configuration class instance with read
        /// data</returns>
        public static Configuration ReadConfigFile(string configFile)
        {
            Configuration configuration = new Configuration();
            XmlTextReader reader = new XmlTextReader(configFile);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "cable_cloud_port":
                            configuration.CableCloudPort = reader.ReadElementContentAsString();
                            break;
                        case "source_port":
                            configuration.SourcePort = reader.ReadElementContentAsString();
                            break;
                        default:
                            break;
                    }
                }
            }
            return configuration;
        }
        
    }
}
