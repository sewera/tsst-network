using System.Collections.Generic;
using System.Threading;
using Common.Models;
using Common.Networking.Client.Persistent;
using Common.Networking.Server.OneShot;
using Common.Startup;
using NetworkNode.Config;
using NetworkNode.Networking.Forwarding;
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

        private readonly Dictionary<string, IOneShotServerPort<RequestPacket, ResponsePacket>> _lrmPorts =
            new Dictionary<string, IOneShotServerPort<RequestPacket, ResponsePacket>>();

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
