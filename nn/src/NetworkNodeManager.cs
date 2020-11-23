using System.Collections.Generic;
using NLog;
using nn.Config;
using nn.Models;
using nn.Networking;
using nn.Networking.Delegates;

namespace nn
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
            foreach (string clientPortAlias in _configuration.ClientPortAliases)
            {
                _clientPorts.Add(clientPortAlias, _clientPortFactory.GetPort(clientPortAlias));
                _clientPorts[clientPortAlias].ConnectToCableCloud();
                _clientPorts[clientPortAlias].StartReceiving();
            }
        }

        public void RegisterReceiveMessageEvent(string clientPortAlias, ReceiveMessageDelegate receiveMessageDelegate)
        {
            _clientPorts[clientPortAlias].RegisterReceiveMessageEvent(receiveMessageDelegate);
        }

        public void Send(string sourcePortAlias, string destinationPortAlias, string message)
        {
            MplsPacket packet = new MplsPacket.Builder()
                .SetSourcePortAlias(sourcePortAlias)
                .SetDestinationPortAlias(destinationPortAlias)
                .SetMplsLabels(_configuration.MplsLabels)
                .SetMessage(message)
                .Build();

            try
            {
                _clientPorts[sourcePortAlias].Send(packet);
            }
            catch (KeyNotFoundException e)
            {
                LOG.Warn($"Port with alias: {sourcePortAlias} was not found in clientPorts dictionary");
            }
        }
    }
}
