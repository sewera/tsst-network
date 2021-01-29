using System.Collections.Generic;
using Common.Models;
using Common.Networking.Client.Persistent;

namespace NetworkNode.Networking.Forwarding
{
    public interface IPacketForwarder
    {
        public void ForwardPacket((string portAlias, EonPacket packet) forwardPacketTuple);
        public void ConfigureFromManagementSystem((string portAlias, ManagementPacket packet) managementTuple);
        public void SetClientPorts(Dictionary<string, IPersistentClientPort<EonPacket>> clientPorts);
    }
}
