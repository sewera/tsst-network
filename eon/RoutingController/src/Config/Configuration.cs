using System.Net;

namespace RoutingController.Config
{
    public class Configuration
    {
        public IPAddress ServerAddress { get; }

        public int RouteTableQueryLocalPort { get; }
        public int LocalTopologyLocalPort { get; }
        public int NetworkTopologyLocalPort { get; }

        private Configuration(IPAddress serverAddress,
                              int routeTableQueryLocalPort,
                              int localTopologyLocalPort,
                              int networkTopologyLocalPort)
        {
            ServerAddress = serverAddress;
            RouteTableQueryLocalPort = routeTableQueryLocalPort;
            LocalTopologyLocalPort = localTopologyLocalPort;
            NetworkTopologyLocalPort = networkTopologyLocalPort;
        }

        public class Builder
        {
            private IPAddress _serverAddress;
            private int _routeTableQueryLocalPort;
            private int _localTopologyLocalPort;
            private int _networkTopologyLocalPort;

            public Builder SetServerAddress(IPAddress serverAddress)
            {
                _serverAddress = serverAddress;
                return this;
            }

            public Builder SetRouteTableQueryLocalPort(int routeTableQueryLocalPort)
            {
                _routeTableQueryLocalPort = routeTableQueryLocalPort;
                return this;
            }

            public Builder SetLocalTopologyLocalPort(int localTopologyLocalPort)
            {
                _localTopologyLocalPort = localTopologyLocalPort;
                return this;
            }

            public Builder SetNetworkTopologyLocalPort(int networkTopologyLocalPort)
            {
                _networkTopologyLocalPort = networkTopologyLocalPort;
                return this;
            }

            public Configuration Build()
            {
                _serverAddress ??= IPAddress.Parse("127.0.0.1");
                return new Configuration(_serverAddress,
                    _routeTableQueryLocalPort,
                    _localTopologyLocalPort,
                    _networkTopologyLocalPort);
            }
        }
    }
}
