using System.Collections.Generic;
using CableCloud.Config;
using CableCloud.Networking.Forwarding;
using Common.Models;
using Common.Networking.Server.Persistent;
using NLog;

namespace CableCloud
{
    public class CableCloudManager : ICableCloudManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly IPersistentServerPort<MplsPacket> _serverPort;
        private readonly IPacketForwarder _packetForwarder;
        private Dictionary<string, IWorker<MplsPacket>> _clientWorkers = new Dictionary<string, IWorker<MplsPacket>>();

        public CableCloudManager(IPersistentServerPort<MplsPacket> serverPort, IPacketForwarder packetForwarder)
        {
            _serverPort = serverPort;
            _packetForwarder = packetForwarder;
        }

        public void Start()
        {
            LOG.Info("CableCloud started");
            _packetForwarder.SetClientPorts(_clientWorkers);
            _serverPort.RegisterRegisterConnectionDelegate(RegisterWorker);
            _serverPort.Listen();
        }

        private void RegisterWorker((string, IWorker<MplsPacket>) worker)
        {
            (string portAlias, IWorker<MplsPacket> clientWorker) = worker;
            _clientWorkers.Add(portAlias, clientWorker);
            clientWorker.RegisterReceiveMessageDelegate(_packetForwarder.ForwardPacket);
            clientWorker.RegisterClientRemovedDelegate(_packetForwarder.OnClientRemoved);
        }

        public void SetConnectionAlive((string, string, bool) requestedConnection)
        {
            _packetForwarder.SetConnectionAlive(requestedConnection);
        }
    }
}
