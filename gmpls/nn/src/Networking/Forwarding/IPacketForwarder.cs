using System.Collections.Generic;
using nn.Models;

namespace nn.Networking.Forwarding
{
    public interface IPacketForwarder
    {
        public void ForwardPacket((string portAlias, MplsPacket packet) forwardPacketTuple);
        public void ConfigureFromManagementSystem((string portAlias, ManagementPacket packet) managementTuple);
        public void SetClientPorts(Dictionary<string, IPort<MplsPacket>> clientPorts);
    }
}
