using nn.src.Models;
using nn.src.Networking.Delegates;

namespace nn.src.Networking
{
    public interface IClientPort
    {
        public void Send(MplsPacket mplsPacket);
        public void ConnectToCableCloud();
        public void StartReceiving();
        public void RegisterReceiveMessageEvent(ReceiveMessage receiveMessage);
    }
}
