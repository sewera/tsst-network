using System.Collections.Generic;
using System.Net;

namespace NetworkCallController.Config
{
    public class Configuration
    {
        public IPAddress ServerAddress { get; }
        public int ConnectionRequestLocalPort { get; }
        public int CallCoordinationLocalPort { get; }
        public int CallTeardownLocalPort { get; }
        public int ConnectionRequestRemotePort { get; }
        public int CallCoordinationRemotePort { get; }
        
        public Dictionary<string, string> ClientPortAliases { get; }

        private Configuration(IPAddress serverAddress,
                              int callCoordinationLocalPort,
                              int callTeardownLocalPort,
                              int connectionRequestLocalPort,
                              int connectionRequestRemotePort,
                              int callCoordinationRemotePort,
                              Dictionary<string, string> clientPortAliases)
        {
            ServerAddress = serverAddress;
            CallCoordinationLocalPort = callCoordinationLocalPort;
            CallTeardownLocalPort = callTeardownLocalPort;
            ConnectionRequestLocalPort = connectionRequestLocalPort;
            ConnectionRequestRemotePort = connectionRequestRemotePort;
            CallCoordinationRemotePort = callCoordinationRemotePort;
            ClientPortAliases = clientPortAliases;
        }

        public class Builder
        {
            private IPAddress _serverAddress;
            private int _connectionRequestLocalPort;
            private int _callCoordinationLocalPort;
            private int _callTeardownLocalPort;
            private int _connectionRequestRemotePort;
            private int _callCoordinationRemotePort;
            private Dictionary<string, string> _clientPortAliases;
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
            
            public Builder SetConnectionRequestRemotePort(int connectionRequestRemotePort)
            {
                _connectionRequestRemotePort = connectionRequestRemotePort;
                return this;
            }
            
            public Builder SetCallCoordinationRemotePort(int callCoordinationRemotePort)
            {
                _callCoordinationRemotePort = callCoordinationRemotePort;
                return this;
            }
            
            public Builder SetClientPortAliases(Dictionary<string, string> clientPortAliases)
            {
                _clientPortAliases = clientPortAliases;
                return this;
            }
            
            public Builder AddClientPortAlias(string clientName, string clientPortAlias)
            {
                _clientPortAliases ??= new Dictionary<string, string>();
                _clientPortAliases.Add(clientName, clientPortAlias);
                return this;
            }

            public Configuration Build()
            {
                _serverAddress ??= IPAddress.Parse("127.0.0.1");
                return new Configuration(_serverAddress,
                    _callCoordinationLocalPort,
                    _callTeardownLocalPort,
                    _connectionRequestLocalPort,
                    _connectionRequestRemotePort,
                    _callCoordinationRemotePort,
                    _clientPortAliases);
            }
        }
    }
}
