using System.Net;

namespace Cc.Config
{
    public class Configuration
    {
        public IPAddress ListeningAddress { get; set; }
        public short ListeningPort { get; set; }

        private Configuration(IPAddress listeningAddress, short listeningPort)
        {
            ListeningAddress = listeningAddress;
            ListeningPort = listeningPort;
        }

        public class Builder
        {
            public Builder SetListeningAddress(string listeningAddress)
            {
                _listeningAddress = IPAddress.Parse(listeningAddress);
                return this;
            }

            public Builder SetListeningPort(short listeningPort)
            {
                _listeningPort = listeningPort;
                return this;
            }

            public Configuration Build()
            {
                return new Configuration(_listeningAddress, _listeningPort);
            }

            private IPAddress _listeningAddress;
            private short _listeningPort;
        }
    }
}
