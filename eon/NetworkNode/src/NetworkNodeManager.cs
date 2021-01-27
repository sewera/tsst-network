using System.Collections.Generic;
using System.Threading;
using Common.Models;
using Common.Networking.Client.Persistent;
using Common.Networking.Server.OneShot;
using Common.Startup;
using NetworkNode.Config;
using NetworkNode.Networking.Forwarding;
using NetworkNode.Networking.LRM;
using NLog;

namespace NetworkNode
{
    public class NetworkNodeManager : IManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly Configuration _configuration;
        private readonly IPacketForwarder _packetForwarder;
        private readonly IPersistentClientPortFactory<MplsPacket> _clientClientPortFactory;
        private readonly Dictionary<string, IPersistentClientPort<MplsPacket>> _clientPorts = new Dictionary<string, IPersistentClientPort<MplsPacket>>();

        private readonly Dictionary<string, LinkResourceManager> _lrmPorts = new Dictionary<string, LinkResourceManager>();

        public NetworkNodeManager(Configuration configuration,
                                  IPacketForwarder packetForwarder,
                                  IPersistentClientPortFactory<MplsPacket> clientClientPortFactory)
        {
            _configuration = configuration;
            _packetForwarder = packetForwarder;
            _clientClientPortFactory = clientClientPortFactory;
        }

        public void Start()
        {
            foreach (string clientPortAlias in _configuration.LocalPortAliases)
            {
                Configuration.LrmConfiguration lrmConfiguration = _configuration.Lrms[clientPortAlias];
                _lrmPorts.Add(clientPortAlias,
                    new LinkResourceManager(clientPortAlias,
                        lrmConfiguration.RemotePortAlias,
                        lrmConfiguration.ServerAddress,
                        lrmConfiguration.LrmLinkConnectionRequestLocalPort,
                        lrmConfiguration.LrmLinkConnectionRequestRemoteAddress,
                        lrmConfiguration.LrmLinkConnectionRequestRemotePort,
                        lrmConfiguration.RcLocalTopologyRemoteAddress,
                        lrmConfiguration.RcLocalTopologyRemotePort));

                _clientPorts.Add(clientPortAlias, _clientClientPortFactory.GetPort(clientPortAlias));
                _clientPorts[clientPortAlias].ConnectPermanentlyToServer(new MplsPacket.Builder().SetSourcePortAlias(clientPortAlias).Build());
                _clientPorts[clientPortAlias].RegisterReceiveMessageEvent(_packetForwarder.ForwardPacket);
                _clientPorts[clientPortAlias].StartReceiving();

                _lrmPorts[clientPortAlias].Listen();
                _lrmPorts[clientPortAlias].SendLocalTopologyPacket();
            }

            _packetForwarder.SetClientPorts(_clientPorts);

            Thread.Sleep(1000);

            _lrmPorts["11"].SendPacket(new RequestPacket.Builder().Build());

            ManualResetEvent allDone = new ManualResetEvent(false);
            allDone.WaitOne();
        }
    }
}
