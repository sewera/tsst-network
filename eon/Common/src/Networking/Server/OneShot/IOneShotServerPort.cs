using Common.Models;
using Common.Networking.Server.Delegates;

namespace Common.Networking.Server.OneShot
{
    public interface IOneShotServerPort<TRequestPacket, in TResponsePacket> : IServerPort<TRequestPacket>
        where TRequestPacket : ISerializablePacket
        where TResponsePacket : ISerializablePacket
    {
        public void RegisterReceiveRequestDelegate(ReceiveRequest<TRequestPacket, TResponsePacket> registerConnectionDelegate);
    }
}
