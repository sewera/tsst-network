using System.Collections.Generic;
using NLog;
using nn.src.Config;
using nn.src.Models;
using nn.src.Networking;
using nn.src.Networking.Delegates;

namespace nn.src
{
    public class NetworkNodeManager : INetworkNodeManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private Configuration _configuration;
        private IClientPortFactory _clientPortFactory;
        private Dictionary<string, IClientPort> _clientPorts;
        private IClientPort _clientPort;

        public NetworkNodeManager(Configuration config, IClientPortFactory clientPortFactory)
        {
            _configuration = config;
            _clientPortFactory = clientPortFactory;
        }

        public void Start()
        {
            // TODO: foreach port add to dictionary
            _clientPort = _clientPortFactory.GetPort(_configuration.ClientPortAliases);
            _clientPort.ConnectToCableCloud();
            _clientPort.StartReceiving();
        }

        public void RegisterReceiveMessageEvent(ReceiveMessage receiveMessage)
        {
            _clientPort.RegisterReceiveMessageEvent(receiveMessage);
        }

        public void Send(string destinationPortAlias, string message)
        {
            MplsPacket packet = new MplsPacket.Builder()
                .SetSourcePortAlias(_configuration.ClientPortAliases)
                .SetDestinationPortAlias(destinationPortAlias)
                .SetMplsLabels(_configuration.MplsLabels)
                .SetMessage(message)
                .Build();

            _clientPort.Send(packet);
        }
    }
}
