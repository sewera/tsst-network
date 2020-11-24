using System.Collections.Generic;
using nn.Models;

namespace nn.Networking.Forwarding
{
    public interface IPacketForwarder
    {
        public void ForwardPacket(MplsPacket packet);
        public void ConfigureFromManagementSystem(ManagementPacket packet);
        public void SetClientPorts(Dictionary<string, IPort<MplsPacket>> clientPorts);
    }
}
