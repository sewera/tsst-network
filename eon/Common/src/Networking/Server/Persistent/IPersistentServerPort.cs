using Common.Models;
using Common.Networking.Server.Delegates;

namespace Common.Networking.Server.Persistent
{
    public interface IPersistentServerPort<TPacket> : IServerPort<TPacket> where TPacket : ISerializablePacket
    {
        public void RegisterRegisterConnectionDelegate(RegisterConnection<TPacket> registerConnectionDelegate);
    }
}
