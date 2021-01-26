using Common.Models;

namespace Common.Networking.Client.Persistent
{
    public interface IPersistentClientPortFactory<TPacket> where TPacket : ISerializablePacket
    {
        public IPersistentClientPort<TPacket> GetPort(string clientPortAlias);
    }
}
