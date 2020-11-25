using System.Collections.Generic;
using NLog;
using nn.Config;
using nn.Models;
using nn.Networking;
using nn.Networking.Client;
using nn.Networking.Forwarding;

namespace nn
{
    public class NetworkNodeManager : INetworkNodeManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly Configuration _configuration;
        private readonly IPacketForwarder _packetForwarder;
        private readonly IClientPortFactory _clientPortFactory;
        private readonly IPort<ManagementPacket> _managementPort;
        private readonly Dictionary<string, IPort<MplsPacket>> _clientPorts = new Dictionary<string, IPort<MplsPacket>>();

        public NetworkNodeManager(Configuration config,
                                  IPacketForwarder packetForwarder,
                                  IPort<ManagementPacket> managementPort,
                                  IClientPortFactory clientPortFactory)
        {
            _configuration = config;
            _packetForwarder = packetForwarder;
            _managementPort = managementPort;
            _clientPortFactory = clientPortFactory;
        }

        public void Start()
        {
            _managementPort.Connect();
            _managementPort.RegisterReceiveMessageEvent(_packetForwarder.ConfigureFromManagementSystem);
            _managementPort.StartReceiving();

            foreach (string clientPortAlias in _configuration.ClientPortAliases)
            {
                _clientPorts.Add(clientPortAlias, _clientPortFactory.GetPort(clientPortAlias));
                _clientPorts[clientPortAlias].Connect();
                _clientPorts[clientPortAlias].RegisterReceiveMessageEvent(_packetForwarder.ForwardPacket);
                _clientPorts[clientPortAlias].StartReceiving();
            }

            _packetForwarder.SetClientPorts(_clientPorts);

            while (true)
            {
                // TODO: Make more elegant solution to program exiting
            }
        }
    }
}
