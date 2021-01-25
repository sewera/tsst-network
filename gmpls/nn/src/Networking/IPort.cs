using nn.Models;
using nn.Networking.Delegates;

namespace nn.Networking
{
    public interface IPort<T> where T : ISerializablePacket
    {
        public void Send(T packet);
        public void Connect();
        public void StartReceiving();
        public void RegisterReceiveMessageEvent(ReceiveMessageDelegate<T> receiveMessageDelegate);
    }
}
