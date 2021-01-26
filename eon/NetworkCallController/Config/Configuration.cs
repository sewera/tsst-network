using System.Net;

namespace NetworkCallController.Config
{
    public class Configuration
    {
        public IPAddress ServerAddress { get; }

        public int CallCoordinationPort { get; }
        public int CallTeardownPort { get; }
        public int ConnectionRequestPort { get; }

        private Configuration(IPAddress serverAddress,
                              int callCoordinationPort,
                              int callTeardownPort,
                              int connectionRequestPort)
        {
            ServerAddress = serverAddress;
            CallCoordinationPort = callCoordinationPort;
            CallTeardownPort = callTeardownPort;
            ConnectionRequestPort = connectionRequestPort;
        }

        public class Builder
        {
            private IPAddress _serverAddress;
            private int _callCoordinationPort;
            private int _callTeardownPort;
            private int _connectionRequestPort;

            public Builder SetServerAddress(IPAddress serverAddress)
            {
                _serverAddress = serverAddress;
                return this;
            }

            public Builder SetCallCoordinationPort(int callCoordinationPort)
            {
                _callCoordinationPort = callCoordinationPort;
                return this;
            }

            public Builder SetCallTeardownPort(int callTeardownPort)
            {
                _callTeardownPort = callTeardownPort;
                return this;
            }

            public Builder SetConnectionRequestPort(int connectionRequestPort)
            {
                _connectionRequestPort = connectionRequestPort;
                return this;
            }

            public Configuration Build()
            {
                _serverAddress ??= IPAddress.Parse("127.0.0.1");
                return new Configuration(_serverAddress,
                    _callCoordinationPort,
                    _callTeardownPort,
                    _connectionRequestPort);
            }
        }
    }
}
