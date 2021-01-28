using System.Collections.Generic;
using System.Net;

namespace ClientNode.Config
{
    public class Configuration
    {
        public IPAddress CallingPartyCallControllerAddress { get; }
        public int CallingPartyCallControllerPort { get; }

        public IPAddress NccConnectionRequestRemoteAddress { get; }
        public int NccConnectionRequestRemotePort { get; }

        public IPAddress NccCallTeardownRemoteAddress { get; }
        public int NccCallTeardownRemotePort { get; }

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
                              IPAddress callingPartyCallControllerAddress,
                              int callingPartyCallControllerPort,
                              IPEndPoint cableCloudEndPoint,
                              string clientAlias,
                              string clientPortAlias,
                              IDictionary<string, long> mplsLabels,
                              IPAddress nccConnectionRequestRemoteAddress,
                              int nccConnectionRequestRemotePort,
                              IPAddress nccCallTeardownRemoteAddress,
                              int nccCallTeardownRemotePort)
        {
            CableCloudAddress = cableCloudAddress;
            CableCloudPort = cableCloudPort;
            CallingPartyCallControllerAddress = callingPartyCallControllerAddress;
            CallingPartyCallControllerPort = callingPartyCallControllerPort;
            CableCloudEndPoint = cableCloudEndPoint;
            ClientAlias = clientAlias;
            ClientPortAlias = clientPortAlias;
            MplsLabels = mplsLabels;
            NccConnectionRequestRemoteAddress = nccConnectionRequestRemoteAddress;
            NccConnectionRequestRemotePort = nccConnectionRequestRemotePort;
            NccCallTeardownRemoteAddress = nccCallTeardownRemoteAddress;
            NccCallTeardownRemotePort = nccCallTeardownRemotePort;
        }

        public class Builder
        {
            private IPAddress _cableCloudAddress;
            private int _cableCloudPort;
            private IPAddress _callingPartyCallControllerAddress;
            private int _callingPartyCallControllerPort;
            private string _clientAlias;
            private string _clientPortAlias = string.Empty;
            private IDictionary<string, long> _mplsLabels;
            private IPAddress _nccConnectionRequestRemoteAddress;
            private int _nccConnectionRequestRemotePort;
            private IPAddress _nccCallTeardownRemoteAddress;
            private int _nccCallTeardownRemotePort;


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
            
            public Builder SetCallingPartyCallControllerAddress(string callingPartyCallControllerAddress)
            {
                _callingPartyCallControllerAddress = IPAddress.Parse(callingPartyCallControllerAddress);
                return this;
            }

            public Builder SetCallingPartyCallControllerPort(int callingPartyCallControllerPort)
            {
                _callingPartyCallControllerPort = callingPartyCallControllerPort;
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

            public Builder SetNccConnectionRequestRemoteAddress(IPAddress nccConnectionRequestRemoteAddress)
            {
                _nccConnectionRequestRemoteAddress = nccConnectionRequestRemoteAddress;
                return this;
            }

            public Builder SetNccConnectionRequestRemotePort(int nccConnectionRequestRemotePort)
            {
                _nccConnectionRequestRemotePort = nccConnectionRequestRemotePort;
                return this;
            }

            public Builder SetNccCallTeardownRemoteAddress(IPAddress nccCallTeardownRemoteAddress)
            {
                _nccCallTeardownRemoteAddress = nccCallTeardownRemoteAddress;
                return this;
            }

            public Builder SetNccCallTeardownRemotePort(int nccCallTeardownRemotePort)
            {
                _nccCallTeardownRemotePort = nccCallTeardownRemotePort;
                return this;
            }

            public Configuration Build()
            {
                _cableCloudAddress ??= IPAddress.Parse("127.0.0.1");
                _callingPartyCallControllerAddress ??= IPAddress.Parse("127.0.0.1");
                _nccConnectionRequestRemoteAddress ??= IPAddress.Parse("127.0.0.1");
                _nccCallTeardownRemoteAddress ??= IPAddress.Parse("127.0.0.1");
                _mplsLabels ??= new Dictionary<string, long>();
                IPEndPoint cableCloudEndPoint = new IPEndPoint(_cableCloudAddress, _cableCloudPort);
                return new Configuration(_cableCloudAddress,
                    _cableCloudPort,
                    _callingPartyCallControllerAddress,
                    _callingPartyCallControllerPort,
                    cableCloudEndPoint,
                    _clientAlias,
                    _clientPortAlias,
                    _mplsLabels,
                    _nccConnectionRequestRemoteAddress,
                    _nccConnectionRequestRemotePort,
                    _nccCallTeardownRemoteAddress,
                    _nccCallTeardownRemotePort);
            }
        }
    }
}
