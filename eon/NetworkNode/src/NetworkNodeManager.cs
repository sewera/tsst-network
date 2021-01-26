using System.Collections.Generic;
using System.Threading;
using Common.Models;
using Common.Networking.Client.Persistent;
using NLog;
using NetworkNode.Config;
using NetworkNode.Networking.Forwarding;

namespace NetworkNode
{
    public class NetworkNodeManager : INetworkNodeManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly Configuration _configuration;
        private readonly IPacketForwarder _packetForwarder;
        private readonly IPersistentClientPortFactory<MplsPacket> _clientClientPortFactory;
        private readonly Dictionary<string, IPersistentClientPort<MplsPacket>> _clientPorts = new Dictionary<string, IPersistentClientPort<MplsPacket>>();

        public NetworkNodeManager(Configuration config,
                                  IPacketForwarder packetForwarder,
                                  IPersistentClientPortFactory<MplsPacket> clientClientPortFactory)
        {
            _configuration = config;
            _packetForwarder = packetForwarder;
            _clientClientPortFactory = clientClientPortFactory;
        }

        public void Start()
        {
            foreach (string clientPortAlias in _configuration.ClientPortAliases)
            {
                _clientPorts.Add(clientPortAlias, _clientClientPortFactory.GetPort(clientPortAlias));
                _clientPorts[clientPortAlias].ConnectPermanentlyToServer(new MplsPacket.Builder().SetSourcePortAlias(clientPortAlias).Build());
                _clientPorts[clientPortAlias].RegisterReceiveMessageEvent(_packetForwarder.ForwardPacket);
                _clientPorts[clientPortAlias].StartReceiving();
            }

            _packetForwarder.SetClientPorts(_clientPorts);
            
            ManualResetEvent allDone = new ManualResetEvent(false);
            allDone.WaitOne();
        }
    }
}
