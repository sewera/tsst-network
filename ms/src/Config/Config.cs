using System.Collections.Generic;
using System.Xml;
using System;
using NLog;

namespace ms
{
    public class Config : IConfig
    {
        /// <summary>
        /// Listener socket port
        /// </summary>
        public short Port { get; set; }
        /// <summary>
        /// List of messages to be sent to network nodes in order to config their MPLS-tables
        /// </summary>
        public List<Message> configMessages = new List<Message>();
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        public Config ReadConfigFile(string fileName)
        {
            Config result = new Config();

            XmlTextReader xtr = new XmlTextReader(fileName);

            List<string> aliases = new List<string>();
            List<string> contents = new List<string>();

            while (xtr.Read())
            {
                if (xtr.NodeType == XmlNodeType.Element )
                {
                    switch (xtr.Name)
                    {
                        case "listener_port":
                            Port = short.Parse(xtr.ReadElementContentAsString());
                            break;
                        case "alias":
                            aliases.Add(xtr.ReadElementContentAsString());
                            break;
                        case "content":
                            contents.Add(xtr.ReadElementContentAsString());
                            break;
                        default:
                            break;
                    }
                }
            }

            for(int i=0;i<Math.Min(aliases.Count,contents.Count);i++)
            {
                configMessages.Add(new Message(aliases[i],contents[i]));
            }

           
            foreach (Message m in configMessages)
            {
                LOG.Info($"Message read: {m.show()}");
            }

            return result;
        }
    }
}