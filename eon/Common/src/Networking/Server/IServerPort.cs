using Common.Models;

namespace Common.Networking.Server
{
    public interface IServerPort<TPacket> where TPacket: ISerializablePacket
    {
        void Listen();
    }
}
