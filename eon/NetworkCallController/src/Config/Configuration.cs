using System.Net;

namespace NetworkCallController.Config
{
    public class Configuration
    {
        public IPAddress ServerAddress { get; }

        public int CallCoordinationLocalPort { get; }
        public int CallTeardownLocalPort { get; }
        public int ConnectionRequestLocalPort { get; }

        private Configuration(IPAddress serverAddress,
                              int callCoordinationLocalPort,
                              int callTeardownLocalPort,
                              int connectionRequestLocalPort)
        {
            ServerAddress = serverAddress;
            CallCoordinationLocalPort = callCoordinationLocalPort;
            CallTeardownLocalPort = callTeardownLocalPort;
            ConnectionRequestLocalPort = connectionRequestLocalPort;
        }

        public class Builder
        {
            private IPAddress _serverAddress;
            private int _callCoordinationLocalPort;
            private int _callTeardownLocalPort;
            private int _connectionRequestLocalPort;

            public Builder SetServerAddress(IPAddress serverAddress)
            {
                _serverAddress = serverAddress;
                return this;
            }

            public Builder SetCallCoordinationLocalPort(int callCoordinationLocalPort)
            {
                _callCoordinationLocalPort = callCoordinationLocalPort;
                return this;
            }

            public Builder SetCallTeardownLocalPort(int callTeardownLocalPort)
            {
                _callTeardownLocalPort = callTeardownLocalPort;
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
                    _callCoordinationLocalPort,
                    _callTeardownLocalPort,
                    _connectionRequestLocalPort);
            }
        }
    }
}
