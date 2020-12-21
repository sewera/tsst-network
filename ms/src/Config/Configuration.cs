using System.Collections.Generic;

namespace ms.Config
{
    public class Configuration
    {
        /// <summary>
        /// Listener socket port
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// List of messages to be sent to network nodes in order to config their MPLS-tables
        /// </summary>
        public List<Message> ConfigMessages;

        private Configuration(int port, List<Message> configMessages)
        {
            Port = port;
            ConfigMessages = configMessages;
        }

        public class Builder
        {
            private int _port;
            private List<Message> _configMessages;

            public Builder SetPort(int port)
            {
                _port = port;
                return this;
            }

            public Builder SetConfigMessages(List<Message> configMessages)
            {
                _configMessages = configMessages;
                return this;
            }

            public Builder AddConfigMessage(Message configMessage)
            {
                _configMessages ??= new List<Message>();
                _configMessages.Add(configMessage);
                return this;
            }

            public Configuration Build()
            {
                _configMessages ??= new List<Message>();
                return new Configuration(_port, _configMessages);
            }
        }
    }
}
