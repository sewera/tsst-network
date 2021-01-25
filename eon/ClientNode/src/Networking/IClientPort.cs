using ClientNetwork.Models;
using ClientNetwork.Networking.Delegates;

namespace ClientNetwork.Networking
{
    public interface IClientPort
    {
        public void Send(MplsPacket mplsPacket);
        public void ConnectToCableCloud();
        public void StartReceiving();
        public void RegisterReceiveMessageEvent(ReceiveMessage receiveMessage);
    }
}
