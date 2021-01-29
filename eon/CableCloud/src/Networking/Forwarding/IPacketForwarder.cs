using System.Collections.Generic;
using Common.Models;
using Common.Networking.Server.Persistent;

namespace CableCloud.Networking.Forwarding
{
    public interface IPacketForwarder
    {
        /// <summary>Forward packet (send it to appropriate client)</summary>
        public void ForwardPacket((string, EonPacket) forwardPacketTuple);
        public void SetClientPorts(IDictionary<string, IWorker<EonPacket>> clientWorkers);
        public void SetConnectionAlive((string, string, bool) requestedConnection);

        public void OnClientRemoved(string key);
    }
}
