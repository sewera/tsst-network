using Common.Models;

namespace Common.Networking.Client.Persistent
{
    public interface IPersistentClientPort<TPacket> : IClientPort<TPacket, TPacket> where TPacket : ISerializablePacket
    {
        public void ConnectPermanentlyToServer(TPacket helloPacket);
        public void StartReceiving();
    }
}
