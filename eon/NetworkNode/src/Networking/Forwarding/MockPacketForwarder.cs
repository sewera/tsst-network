using System.Collections.Generic;
using Common.Models;
using Common.Networking.Client.Persistent;
using NLog;

namespace NetworkNode.Networking.Forwarding
{
    public class MockPacketForwarder : IPacketForwarder
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private Dictionary<string, IPersistentClientPort<MplsPacket>> _clientPorts;

        public void ForwardPacket((string portAlias, MplsPacket packet) forwardPacketTuple)
        {
            (string portAlias, MplsPacket packet) = forwardPacketTuple;
            if (_clientPorts == null)
            {
                LOG.Warn("Dictionary with clientPorts was not initialized yet");
                return;
            }

            packet.MplsLabels = new List<long> {200, 300};
            const string destinationLocalPortAlias = "12";
            LOG.Debug($"Sending packet {packet} via {destinationLocalPortAlias}");
            _clientPorts[destinationLocalPortAlias].Send(packet);
        }

        public void ConfigureFromManagementSystem((string portAlias, ManagementPacket packet) managementTuple)
        {
            (string portAlias, ManagementPacket packet) = managementTuple;
            LOG.Info($"Received command from MS: {packet}");
        }

        public void SetClientPorts(Dictionary<string, IPersistentClientPort<MplsPacket>> clientPorts)
        {
            _clientPorts = clientPorts;
        }
    }
}
