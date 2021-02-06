using System.Collections.Generic;
using System.Net;

namespace RoutingController.Config
{
    public class Configuration
    {
        public IPAddress ServerAddress { get; }

        public string RoutingControllerAlias { get; }
        public int RouteTableQueryLocalPort { get; }
        public int LocalTopologyLocalPort { get; }
        public int NetworkTopologyLocalPort { get; }
        public List<RouteTableRow> RouteTable { get; }

        private Configuration(IPAddress serverAddress,
                              string routingControllerAlias,
                              int routeTableQueryLocalPort,
                              int localTopologyLocalPort,
                              int networkTopologyLocalPort,
                              List<RouteTableRow> routeTable)
        {
            ServerAddress = serverAddress;
            RoutingControllerAlias = routingControllerAlias;
            RouteTableQueryLocalPort = routeTableQueryLocalPort;
            LocalTopologyLocalPort = localTopologyLocalPort;
            NetworkTopologyLocalPort = networkTopologyLocalPort;
            RouteTable = routeTable;
        }

        public class Builder
        {
            private IPAddress _serverAddress;
            private string _routingControllerAlias;
            private int _routeTableQueryLocalPort;
            private int _localTopologyLocalPort;
            private int _networkTopologyLocalPort;
            private List<RouteTableRow> _routeTable;

            public Builder SetServerAddress(IPAddress serverAddress)
            {
                _serverAddress = serverAddress;
                return this;
            }

            public Builder SetRoutingControllerAlias(string routingControllerAlias)
            {
                _routingControllerAlias = routingControllerAlias;
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

            public Builder SetRouteTable(List<RouteTableRow> routeTable)
            {
                _routeTable = routeTable;
                return this;
            }

            public Builder AddRouteTableRow(RouteTableRow routeTableRow)
            {
                _routeTable ??= new List<RouteTableRow>();
                _routeTable.Add(routeTableRow);
                return this;
            }

            public Configuration Build()
            {
                _serverAddress ??= IPAddress.Parse("127.0.0.1");
                return new Configuration(_serverAddress,
                    _routingControllerAlias,
                    _routeTableQueryLocalPort,
                    _localTopologyLocalPort,
                    _networkTopologyLocalPort,
                    _routeTable);
            }
        }

        public class RouteTableRow
        {
            public string Src { get; }
            public string Dst { get; }
            public string Gateway { get; }

            public RouteTableRow(string src, string dst, string gateway)
            {
                Src = src;
                Dst = dst;
                Gateway = gateway;
            }

            public class RouteTableRowBuilder
            {
                private string _src;
                private string _dst;
                private string _gateway;

                public RouteTableRowBuilder SetSrc(string src)
                {
                    _src = src;
                    return this;
                }

                public RouteTableRowBuilder SetDst(string dst)
                {
                    _dst = dst;
                    return this;
                }

                public RouteTableRowBuilder SetGateway(string gateway)
                {
                    _gateway = gateway;
                    return this;
                }

                public RouteTableRow Build()
                {
                    return new RouteTableRow(_src,
                        _dst,
                        _gateway);
                }
            }
        }
    }
}
