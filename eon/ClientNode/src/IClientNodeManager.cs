using System.Collections.Generic;
using Common.Models;
using Common.Networking.Client.Delegates;

namespace ClientNode
{
    public interface IClientNodeManager
    {
        public void Start();
        public void RegisterReceiveMessageEvent(ReceiveMessage<MplsPacket> receiveMessage);
        public void Send(string mplsOutLabel, string message, (List<long>, string) labels);
    }
}
