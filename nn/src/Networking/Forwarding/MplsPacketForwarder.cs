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

        public void ForwardPacket(MplsPacket packet)
        {
            if (_clientPorts == null)
            {
                LOG.Warn("Dictionary with clientPorts was not initialized yet");
                return;
            }

            // TODO: Implementation
            throw new NotImplementedException();
        }

        public void ConfigureFromManagementSystem(ManagementPacket packet)
        {
            // TODO: Implementation
            throw new NotImplementedException();
        }

        public void SetClientPorts(Dictionary<string, IPort<MplsPacket>> clientPorts)
        {
            _clientPorts = clientPorts;
        }
    }
}
