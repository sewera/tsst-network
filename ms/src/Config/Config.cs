using System.Collections.Generic;
using System.Xml;
using System;

namespace ms
{
    public class Config : IConfig
    {
        /// <summary>
        /// Listener Port for Management Manager
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// List of messages that has to be sent
        /// </summary>
        public List<Message> configMessages = new List<Message>();

        /// <summary>
        /// Default class constructor
        /// </summary>
        public Config()
        {
            ;;
        }
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
                            Port = Int32.Parse(xtr.ReadElementContentAsString());
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

            // <Temporarily here>
            Console.WriteLine("Messages read from config file:");

            foreach (Message m in configMessages)
            {
                m.show();
            }
            // </Temporarily here>

            return result;
        }
    }
}