using ClientNode.Models;
using ClientNode.Networking.Delegates;

namespace ClientNode.Networking
{
    public interface IClientPort
    {
        public void Send(MplsPacket mplsPacket);
        public void ConnectToCableCloud();
        public void StartReceiving();
        public void RegisterReceiveMessageEvent(ReceiveMessage receiveMessage);
    }
}
