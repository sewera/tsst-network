using System.Collections.Generic;
using cc.Models;
using cc.Networking.Client;

namespace cc.Networking.Forwarding
{
    public interface IPacketForwarder
    {
        /// <summary>Forward packet (send it to appropriate client)</summary>
        public void ForwardPacket((string, MplsPacket) forwardPacketTuple);
        public void SetClientPorts(Dictionary<string, IClientWorker> clientWorkers);
        public void SetConnectionAlive((string, string, bool) requestedConnection);

        public void OnClientRemoved(object source, ClientRemovedEventArgs eventArgs);
    }
}
