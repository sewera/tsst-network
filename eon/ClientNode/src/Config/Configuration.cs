using System.Collections.Generic;
using System.Net;

namespace ClientNode.Config
{
    public class Configuration
    {
        /// <summary>CableCloud address</summary>
        public IPAddress CableCloudAddress { get; }

        /// <summary>CableCloud port to connect to</summary>
        public int CableCloudPort { get; }

        /// <summary>IPEndPoint combining CableCloudAddress and CableCloudPort</summary>
        public IPEndPoint CableCloudEndPoint { get; }
        
        /// <summary>Alias representation of client</summary>
        public string ClientAlias { get;  }

        /// <summary>Alias representation host client node port</summary>
        public string ClientPortAlias { get; }

        /// <summary>Dictionary of MPLS labels with key being host alias</summary>
        public IDictionary<string, long> MplsLabels { get; }

        private Configuration(IPAddress cableCloudAddress,
                              int cableCloudPort,
                              IPEndPoint cableCloudEndPoint,
                              string clientAlias,
                              string clientPortAlias,
                              IDictionary<string, long> mplsLabels)
        {
            CableCloudAddress = cableCloudAddress;
            CableCloudPort = cableCloudPort;
            CableCloudEndPoint = cableCloudEndPoint;
            ClientAlias = clientAlias;
            ClientPortAlias = clientPortAlias;
            MplsLabels = mplsLabels;
        }

        public class Builder
        {
            private IPAddress _cableCloudAddress;
            private int _cableCloudPort;
            private string _clientAlias;
            private string _clientPortAlias = string.Empty;
            private IDictionary<string, long> _mplsLabels;

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
            
            public Builder SetClientAlias(string clientAlias)
            {
                _clientAlias = clientAlias;
                return this;
            }

            public Builder SetClientPortAlias(string clientPortAlias)
            {
                _clientPortAlias = clientPortAlias;
                return this;
            }

            public Builder AddMplsLabel(string hostAlias, long mplsLabel)
            {
                _mplsLabels ??= new Dictionary<string, long>();
                _mplsLabels.Add(hostAlias, mplsLabel);
                return this;
            }

            public Builder SetMplsLabels(IDictionary<string, long> mplsLabels)
            {
                _mplsLabels = mplsLabels;
                return this;
            }

            public Configuration Build()
            {
                _cableCloudAddress ??= IPAddress.Parse("127.0.0.1");
                _mplsLabels ??= new Dictionary<string, long>();
                IPEndPoint cableCloudEndPoint = new IPEndPoint(_cableCloudAddress, _cableCloudPort);
                return new Configuration(_cableCloudAddress,
                    _cableCloudPort,
                    cableCloudEndPoint,
                    _clientAlias,
                    _clientPortAlias,
                    _mplsLabels);
            }
        }
    }
}
