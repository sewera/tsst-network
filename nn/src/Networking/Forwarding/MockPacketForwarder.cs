using System.Collections.Generic;
using NLog;
using nn.Config;
using nn.Models;

namespace nn.Networking.Forwarding
{
    public class MockPacketForwarder : IPacketForwarder
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Configuration _configuration;
        private Dictionary<string, IPort<MplsPacket>> _clientPorts;

        public MockPacketForwarder(Configuration configuration)
        {
            _configuration = configuration;
        }

        public void ForwardPacket(MplsPacket packet)
        {
            if (_clientPorts == null)
            {
                LOG.Warn("Dictionary with clientPorts was not initialized yet");
                return;
            }

            packet.MplsLabels = new List<long> {200, 300};
            const string destinationLocalPortAlias = "R1/2";
            LOG.Debug($"Sending packet {packet} via {destinationLocalPortAlias}");
            _clientPorts[destinationLocalPortAlias].Send(packet);
        }

        public void ConfigureFromManagementSystem(ManagementPacket packet)
        {
            LOG.Info($"Received command from MS: {packet}");
        }

        public void SetClientPorts(Dictionary<string, IPort<MplsPacket>> clientPorts)
        {
            _clientPorts = clientPorts;
        }
    }
}
