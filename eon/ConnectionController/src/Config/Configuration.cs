using System.Net;

namespace ConnectionController.Config
{
    public class Configuration
    {
        public IPAddress ServerAddress { get; }

        public int PeerCoordinationLocalPort { get; }
        public int ConnectionRequestLocalPort { get; }

        private Configuration(IPAddress serverAddress,
                              int peerCoordinationLocalPort,
                              int connectionRequestLocalPort)
        {
            ServerAddress = serverAddress;
            PeerCoordinationLocalPort = peerCoordinationLocalPort;
            ConnectionRequestLocalPort = connectionRequestLocalPort;
        }

        public class Builder
        {
            private IPAddress _serverAddress;
            private int _peerCoordinationLocalPort;
            private int _connectionRequestLocalPort;

            public Builder SetServerAddress(IPAddress serverAddress)
            {
                _serverAddress = serverAddress;
                return this;
            }

            public Builder SetPeerCoordinationLocalPort(int peerCoordinationLocalPort)
            {
                _peerCoordinationLocalPort = peerCoordinationLocalPort;
                return this;
            }

            public Builder SetConnectionRequestLocalPort(int connectionRequestLocalPort)
            {
                _connectionRequestLocalPort = connectionRequestLocalPort;
                return this;
            }

            public Configuration Build()
            {
                _serverAddress ??= IPAddress.Parse("127.0.0.1");
                return new Configuration(_serverAddress,
                    _peerCoordinationLocalPort,
                    _connectionRequestLocalPort);
            }
        }
    }
}
