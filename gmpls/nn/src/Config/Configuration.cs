using System.Collections.Generic;
using System.Net;

namespace nn.Config
{
    public class Configuration
    {
        public string RouterAlias { get; }

        /// <summary>CableCloud address</summary>
        public IPAddress CableCloudAddress { get; }

        /// <summary>CableCloud port to connect to</summary>
        public int CableCloudPort { get; }

        /// <summary>IPEndPoint combining CableCloudAddress and CableCloudPort</summary>
        public IPEndPoint CableCloudEndPoint { get; }

        /// <summary>CableCloud address</summary>
        public IPAddress ManagementSystemAddress { get; }

        /// <summary>CableCloud port to connect to</summary>
        public int ManagementSystemPort { get; }

        /// <summary>IPEndPoint combining CableCloudAddress and CableCloudPort</summary>
        public IPEndPoint ManagementSystemEndPoint { get; }

        /// <summary>Alias representation host client node port</summary>
        public List<string> ClientPortAliases { get; }

        private Configuration(string routerAlias,
                              IPAddress cableCloudAddress,
                              int cableCloudPort,
                              IPEndPoint cableCloudEndPoint,
                              IPAddress managementSystemAddress,
                              int managementSystemPort,
                              IPEndPoint managementSystemEndPoint,
                              List<string> clientPortAliases)
        {
            RouterAlias = routerAlias;
            CableCloudAddress = cableCloudAddress;
            CableCloudPort = cableCloudPort;
            CableCloudEndPoint = cableCloudEndPoint;
            ManagementSystemAddress = managementSystemAddress;
            ManagementSystemPort = managementSystemPort;
            ManagementSystemEndPoint = managementSystemEndPoint;
            ClientPortAliases = clientPortAliases;
        }

        public class Builder
        {
            private string _routerAlias;
            private IPAddress _cableCloudAddress;
            private int _cableCloudPort;
            private IPAddress _managementSystemAddress;
            private int _managementSystemPort;
            private List<string> _clientPortAliases;

            public Builder SetRouterAlias(string routerAlias)
            {
                _routerAlias = routerAlias;
                return this;
            }

            public Builder SetCableCloudAddress(string cableCloudAddress)
            {
                _cableCloudAddress = IPAddress.Parse(cableCloudAddress);
                return this;
            }

            public Builder SetCableCloudPort(int cableCloudPort)
            {
                _cableCloudPort = cableCloudPort;
                return this;
            }

            public Builder SetManagementSystemAddress(string managementSystemAddress)
            {
                _managementSystemAddress = IPAddress.Parse(managementSystemAddress);
                return this;
            }

            public Builder SetManagementSystemPort(int managementSystemPort)
            {
                _managementSystemPort = managementSystemPort;
                return this;
            }

            public Builder SetClientPortAliases(List<string> clientPortAliases)
            {
                _clientPortAliases = clientPortAliases;
                return this;
            }

            public Builder AddPortAlias(string clientPortAlias)
            {
                _clientPortAliases ??= new List<string>();
                _clientPortAliases.Add(clientPortAlias);
                return this;
            }

            public Configuration Build()
            {
                _cableCloudAddress ??= IPAddress.Parse("127.0.0.1");
                _managementSystemAddress ??= IPAddress.Parse("127.0.0.1");
                _clientPortAliases ??= new List<string>();
                IPEndPoint cableCloudEndPoint = new IPEndPoint(_cableCloudAddress,
                    _cableCloudPort);
                IPEndPoint managementSystemEndPoint = new IPEndPoint(_managementSystemAddress,
                    _managementSystemPort);
                return new Configuration(_routerAlias,
                    _cableCloudAddress,
                    _cableCloudPort,
                    cableCloudEndPoint,
                    _managementSystemAddress,
                    _managementSystemPort,
                    managementSystemEndPoint,
                    _clientPortAliases);
            }
        }
    }
}
