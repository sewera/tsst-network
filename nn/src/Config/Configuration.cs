using System.Collections.Generic;
using System.Net;

namespace nn.Config
{
    public class Configuration
    {
        /// <summary>CableCloud address</summary>
        public IPAddress CableCloudAddress { get; }

        /// <summary>CableCloud port to connect to</summary>
        public int CableCloudPort { get; }

        /// <summary>IPEndPoint combining CableCloudAddress and CableCloudPort</summary>
        public IPEndPoint CableCloudEndPoint { get; }

        /// <summary>Alias representation host client node port</summary>
        public List<string> ClientPortAliases { get; }

        /// <summary>List of MPLS labels</summary>
        public List<long> MplsLabels { get; }

        private Configuration(IPAddress cableCloudAddress,
                              int cableCloudPort,
                              IPEndPoint cableCloudEndPoint,
                              List<string> clientPortAliases,
                              List<long> mplsLabels)
        {
            CableCloudAddress = cableCloudAddress;
            CableCloudPort = cableCloudPort;
            CableCloudEndPoint = cableCloudEndPoint;
            ClientPortAliases = clientPortAliases;
            MplsLabels = mplsLabels;
        }

        public class Builder
        {
            private IPAddress _cableCloudAddress;
            private int _cableCloudPort;
            private List<string> _clientPortAliases;
            private List<long> _mplsLabels;

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

            public Builder SetClientPortAliases(List<string> clientPortAliases)
            {
                _clientPortAliases = clientPortAliases;
                return this;
            }

            public Builder AddClientPortAlias(string clientPortAlias)
            {
                _clientPortAliases ??= new List<string>();
                _clientPortAliases.Add(clientPortAlias);
                return this;
            }

            public Builder AddMplsLabel(long mplsLabel)
            {
                _mplsLabels ??= new List<long>();
                _mplsLabels.Add(mplsLabel);
                return this;
            }

            public Builder SetMplsLabels(List<long> mplsLabels)
            {
                _mplsLabels = mplsLabels;
                return this;
            }

            public Configuration Build()
            {
                _cableCloudAddress ??= IPAddress.Parse("127.0.0.1");
                _mplsLabels ??= new List<long>();
                _clientPortAliases ??= new List<string>();
                IPEndPoint cableCloudEndPoint = new IPEndPoint(_cableCloudAddress, _cableCloudPort);
                return new Configuration(_cableCloudAddress,
                    _cableCloudPort,
                    cableCloudEndPoint,
                    _clientPortAliases,
                    _mplsLabels);
            }
        }
    }
}
