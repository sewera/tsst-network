using System.Collections.Generic;
using ClientNode.Networking.Delegates;

namespace ClientNode
{
    public interface IClientNodeManager
    {
        public void Start();
        public void RegisterReceiveMessageEvent(ReceiveMessage receiveMessage);
        public void Send(string mplsOutLabel, string message, (List<long>, string) labels);
    }
}
