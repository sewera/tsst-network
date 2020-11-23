using nn.Models;
using nn.Networking.Delegates;

namespace nn.Networking
{
    public interface IClientPort
    {
        public void Send(MplsPacket mplsPacket);
        public void ConnectToCableCloud();
        public void StartReceiving();
        public void RegisterReceiveMessageEvent(ReceiveMessageDelegate receiveMessage);
    }
}
