using Common.Models;
using Common.Networking.Client.Delegates;

namespace Common.Networking.Client
{
    public interface IClientPort<TPacket> where TPacket: ISerializablePacket
    {
        public void Send(TPacket packet);
        public void ConnectPermanentlyToServer();
        public void StartReceiving();
        public void RegisterReceiveMessageEvent(ReceiveMessage<TPacket> receiveMessage);
    }
}
