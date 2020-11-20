using cn.Models;
using cn.Networking.Delegates;

namespace cn.Networking
{
    public interface IClientPort
    {
        public void Send(MplsPacket mplsPacket);
        public void ConnectToCableCloud();
        public void StartReceiving();
        public void RegisterReceiveMessageEvent(ReceiveMessage receiveMessage);
    }
}
