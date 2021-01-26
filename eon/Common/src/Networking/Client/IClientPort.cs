using Common.Models;
using Common.Networking.Client.Delegates;

namespace Common.Networking.Client
{
    public interface IClientPort<in TRequestPacket, TResponsePacket>
        where TRequestPacket : ISerializablePacket
        where TResponsePacket : ISerializablePacket
    {
        public void Send(TRequestPacket requestPacket);
        public void RegisterReceiveMessageEvent(ReceiveMessage<TResponsePacket> receiveMessage);
    }
}
