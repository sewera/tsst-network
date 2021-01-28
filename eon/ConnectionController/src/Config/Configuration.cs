using System.Collections.Generic;
using System.Net;

namespace ConnectionController.Config
{
    public class Configuration
    {
        public IPAddress ServerAddress { get; }
        public string ConnectionControllerType { get; }
        
        public int PeerCoordinationLocalPort { get; }
        public int ConnectionRequestLocalPort { get; }
        
        public Dictionary<string, string> CcNames { get; }
        
        public Dictionary<string, int> CcConnectionRequestRemotePorts { get; }
        
        public Dictionary<string, int> CcPeerCoordinationRemotePorts { get; }

        private Configuration(IPAddress serverAddress,
                              string connectionControllerType,
                              int peerCoordinationLocalPort,
                              int connectionRequestLocalPort,
                              Dictionary<string, string> ccNames,
                              Dictionary<string,int> ccConnectionRequestRemotePorts,
                              Dictionary<string, int> ccPeerCoordinationRemotePorts)
        {
            ServerAddress = serverAddress;
            ConnectionControllerType = connectionControllerType;
            PeerCoordinationLocalPort = peerCoordinationLocalPort;
            ConnectionRequestLocalPort = connectionRequestLocalPort;
            CcNames = ccNames;
            CcConnectionRequestRemotePorts = ccConnectionRequestRemotePorts;
            CcPeerCoordinationRemotePorts = ccPeerCoordinationRemotePorts;
        }

        public class Builder
        {
            private IPAddress _serverAddress;
            private string _connectionControllerType;
            private int _peerCoordinationLocalPort;
            private int _connectionRequestLocalPort;
            private Dictionary<string, string> _ccNames;
            private Dictionary<string, int> _ccConnectionRequestRemotePorts;
            private Dictionary<string, int> _ccPeerCoordinationRemotePorts;

            public Builder SetServerAddress(IPAddress serverAddress)
            {
                _serverAddress = serverAddress;
                return this;
            }
            
            public Builder SetConnectionControllerType(string connectionControllerType)
            {
                _connectionControllerType = connectionControllerType;
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
            
            public Builder AddCcName(string portPattern, string ccName)
            {
                _ccNames ??= new Dictionary<string, string>();
                _ccNames.Add(portPattern, ccName);
                return this;
            }

            public Builder SetCcNames(Dictionary<string, string> ccNames)
            {
                _ccNames = ccNames;
                return this;
            }
            
            public Builder AddCcConnectionRequestRemotePort(string ccName, int port)
            {
                _ccConnectionRequestRemotePorts ??= new Dictionary<string, int>();
                _ccConnectionRequestRemotePorts.Add(ccName, port);
                return this;
            }

            public Builder SetCcConnectionRequestRemotePorts(Dictionary<string, int> ccPeerCoordinationRemotePorts)
            {
                _ccPeerCoordinationRemotePorts = ccPeerCoordinationRemotePorts;
                return this;
            }
            
            public Builder AddCcPeerCoordinationRemotePort(string ccName, int port)
            {
                _ccPeerCoordinationRemotePorts ??= new Dictionary<string, int>();
                _ccPeerCoordinationRemotePorts.Add(ccName, port);
                return this;
            }

            public Builder SetCcPeerCoordinationRemotePorts(Dictionary<string, int> ccPeerCoordinationRemotePorts)
            {
                _ccPeerCoordinationRemotePorts = ccPeerCoordinationRemotePorts;
                return this;
            }

            public Configuration Build()
            {
                _serverAddress ??= IPAddress.Parse("127.0.0.1");
                _ccNames ??= new Dictionary<string, string>();
                _ccConnectionRequestRemotePorts ??= new Dictionary<string, int>();
                _ccPeerCoordinationRemotePorts ??= new Dictionary<string, int>();

                return new Configuration(_serverAddress,
                    _connectionControllerType,
                    _peerCoordinationLocalPort,
                    _connectionRequestLocalPort,
                    _ccNames,
                    _ccConnectionRequestRemotePorts,
                    _ccPeerCoordinationRemotePorts);
            }
        }
    }
}
