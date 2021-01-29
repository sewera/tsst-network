using System;
using System.Collections.Generic;
using System.Net;

namespace NetworkNode.Config
{
    public class Configuration
    {
        public string RouterAlias { get; }

        public IPAddress CableCloudAddress { get; }
        public int CableCloudPort { get; }

        public Dictionary<string, LrmConfiguration> Lrms { get; }

        public List<string> LocalPortAliases { get; }

        public int NnFibInsertLocalPort { get; }

        private Configuration(string routerAlias,
                              IPAddress cableCloudAddress,
                              int cableCloudPort,
                              Dictionary<string, LrmConfiguration> lrms,
                              List<string> localPortAliases,
                              int nnFibInsertLocalPort)
        {
            RouterAlias = routerAlias;
            CableCloudAddress = cableCloudAddress;
            CableCloudPort = cableCloudPort;
            Lrms = lrms;
            LocalPortAliases = localPortAliases;
            NnFibInsertLocalPort = nnFibInsertLocalPort;
        }

        public class LrmConfiguration
        {
            public string RemotePortAlias;

            public IPAddress ServerAddress;
            public int LrmLinkConnectionRequestLocalPort;

            public IPAddress LrmLinkConnectionRequestRemoteAddress;
            public int LrmLinkConnectionRequestRemotePort;

            public IPAddress RcLocalTopologyRemoteAddress;
            public int RcLocalTopologyRemotePort;

            public class LrmBuilder
            {
                private string _remotePortAlias = string.Empty;

                private IPAddress _serverAddress;
                private int _lrmLinkConnectionRequestLocalPort;

                private IPAddress _lrmLinkConnectionRequestRemoteAddress;
                private int _lrmLinkConnectionRequestRemotePort;

                private IPAddress _rcLocalTopologyRemoteAddress;
                private int _rcLocalTopologyRemotePort;

                public LrmBuilder SetRemotePortAlias(string remotePortAlias)
                {
                    _remotePortAlias = remotePortAlias;
                    return this;
                }

                public LrmBuilder SetServerAddress(IPAddress serverAddress)
                {
                    _serverAddress = serverAddress;
                    return this;
                }

                public LrmBuilder SetLrmLinkConnectionRequestLocalPort(int lrmLinkConnectionRequestLocalPort)
                {
                    _lrmLinkConnectionRequestLocalPort = lrmLinkConnectionRequestLocalPort;
                    return this;
                }

                public LrmBuilder SetLrmLinkConnectionRequestRemoteAddress(IPAddress lrmLinkConnectionRequestRemoteAddress)
                {
                    _lrmLinkConnectionRequestRemoteAddress = lrmLinkConnectionRequestRemoteAddress;
                    return this;
                }

                public LrmBuilder SetLrmLinkConnectionRequestRemotePort(int lrmLinkConnectionRequestRemotePort)
                {
                    _lrmLinkConnectionRequestRemotePort = lrmLinkConnectionRequestRemotePort;
                    return this;
                }

                public LrmBuilder SetRcLocalTopologyRemoteAddress(IPAddress rcLocalTopologyRemoteAddress)
                {
                    _rcLocalTopologyRemoteAddress = rcLocalTopologyRemoteAddress;
                    return this;
                }

                public LrmBuilder SetRcLocalTopologyRemotePort(int rcLocalTopologyRemotePort)
                {
                    _rcLocalTopologyRemotePort = rcLocalTopologyRemotePort;
                    return this;
                }

                public LrmConfiguration Build()
                {
                    _serverAddress ??= IPAddress.Parse("127.0.0.1");
                    _lrmLinkConnectionRequestRemoteAddress ??= IPAddress.Parse("127.0.0.1");
                    _rcLocalTopologyRemoteAddress ??= IPAddress.Parse("127.0.0.1");
                    return new LrmConfiguration
                    {
                        RemotePortAlias = _remotePortAlias,
                        ServerAddress = _serverAddress,
                        LrmLinkConnectionRequestLocalPort = _lrmLinkConnectionRequestLocalPort,
                        LrmLinkConnectionRequestRemoteAddress = _lrmLinkConnectionRequestRemoteAddress,
                        LrmLinkConnectionRequestRemotePort = _lrmLinkConnectionRequestRemotePort,
                        RcLocalTopologyRemoteAddress = _rcLocalTopologyRemoteAddress,
                        RcLocalTopologyRemotePort = _rcLocalTopologyRemotePort
                    };
                }
            }
        }

        public class Builder
        {
            private string _routerAlias;

            private IPAddress _cableCloudAddress;
            private int _cableCloudPort;

            private Dictionary<string, LrmConfiguration> _lrms;

            private List<string> _localPortAliases;
            private int _nnFibInsertLocalPort;

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

            public Builder SetLrms(Dictionary<string, LrmConfiguration> lrms)
            {
                _lrms = lrms;
                return this;
            }

            public Builder AddLrm(string key, LrmConfiguration lrmConfiguration)
            {
                _lrms ??= new Dictionary<string, LrmConfiguration>();
                _lrms.Add(key, lrmConfiguration);
                return this;
            }

            public Builder SetLocalPortAliases(List<string> localPortAliases)
            {
                _localPortAliases = localPortAliases;
                return this;
            }

            public Builder AddPortAlias(string clientPortAlias)
            {
                _localPortAliases ??= new List<string>();
                _localPortAliases.Add(clientPortAlias);
                return this;
            }

            public Builder SetNnFibInsertLocalPort(int nnFibInsertLocalPort)
            {
                _nnFibInsertLocalPort = nnFibInsertLocalPort;
                return this;
            }

            public Configuration Build()
            {
                _cableCloudAddress ??= IPAddress.Parse("127.0.0.1");
                _localPortAliases ??= new List<string>();
                _lrms ??= new Dictionary<string, LrmConfiguration>();
                return new Configuration(_routerAlias,
                    _cableCloudAddress,
                    _cableCloudPort,
                    _lrms,
                    _localPortAliases,
                    _nnFibInsertLocalPort);
            }
        }
    }
}
