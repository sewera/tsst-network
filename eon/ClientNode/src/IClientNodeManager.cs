using System.Collections.Generic;
using Common.Models;
using Common.Networking.Client.Delegates;
using Common.Startup;

namespace ClientNode
{
    public interface IClientNodeManager : IManager
    {
        public void RegisterReceiveMessageEvent(ReceiveMessage<EonPacket> receiveMessage);
        public void Send(string message, string connection);
    }
}
