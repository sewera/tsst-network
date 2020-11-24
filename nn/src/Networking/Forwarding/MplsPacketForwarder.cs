using System;
using System.Collections.Generic;
using NLog;
using nn.Config;
using nn.Models;

namespace nn.Networking.Forwarding
{
    public class MplsPacketForwarder : IPacketForwarder
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Configuration _configuration;
        private Dictionary<string, IPort<MplsPacket>> _clientPorts;

        public MplsPacketForwarder(Configuration configuration)
        {
            _configuration = configuration;
            // TODO: Parse configuration into table (list of dictionaries?)
        }

        public void ForwardPacket((string portAlias, MplsPacket packet) forwardPacketTuple)
        {
            (string portAlias, MplsPacket packet) = forwardPacketTuple;
            if (_clientPorts == null)
            {
                LOG.Warn("Dictionary with clientPorts was not initialized yet");
                return;
            }

            // TODO: Implementation
            LOG.Fatal($"ForwardPacket is not implemented. Packet: {packet}");
            throw new NotImplementedException();
        }

        public void ConfigureFromManagementSystem((string portAlias, ManagementPacket packet) managementTuple)
        {
            (string portAlias, ManagementPacket packet) = managementTuple;
            // TODO: Implementation
            LOG.Fatal($"ConfigureFromManagementSystem is not implemented. Packet: {packet}");
            throw new NotImplementedException();
        }

        public void SetClientPorts(Dictionary<string, IPort<MplsPacket>> clientPorts)
        {
            _clientPorts = clientPorts;
        }
    }
}
