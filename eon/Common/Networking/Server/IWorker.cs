using Common.Models;
using Common.Networking.Server.Delegates;

namespace Common.Networking.Server
{
    public interface IWorker<TPacket> where TPacket: ISerializablePacket
    {
        public void Send(TPacket mplsPacket);
        public void RegisterReceiveMessageDelegate(ReceiveMessage<TPacket> receiveMessageDelegate);
        public void RegisterClientRemovedDelegate(HandleClientRemoval handleClientRemovalDelegate);
    }
}
