using System.Collections.Generic;
using CableCloud.Networking.Client;
using Common.Models;
using Common.Networking.Server.Persistent;

namespace CableCloud.Networking.Forwarding
{
    public interface IPacketForwarder
    {
        /// <summary>Forward packet (send it to appropriate client)</summary>
        public void ForwardPacket((string, MplsPacket) forwardPacketTuple);
        public void SetClientPorts(Dictionary<string, IWorker<MplsPacket>> clientWorkers);
        public void SetConnectionAlive((string, string, bool) requestedConnection);

        public void OnClientRemoved(string key);
    }
}
