using System.Collections.Generic;
using Common.Models;
using Common.Networking.Client.Delegates;
using Common.Startup;

namespace ClientNode
{
    public interface IClientNodeManager : IManager
    {
        public void RegisterReceiveMessageEvent(ReceiveMessage<MplsPacket> receiveMessage);
        public void Send(string mplsOutLabel, string message, (List<long>, string) labels);
    }
}
