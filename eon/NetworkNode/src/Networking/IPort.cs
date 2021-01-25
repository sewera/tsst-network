using NetworkNode.Models;
using NetworkNode.Networking.Delegates;

namespace NetworkNode.Networking
{
    public interface IPort<T> where T : ISerializablePacket
    {
        public void Send(T packet);
        public void Connect();
        public void StartReceiving();
        public void RegisterReceiveMessageEvent(ReceiveMessageDelegate<T> receiveMessageDelegate);
    }
}
