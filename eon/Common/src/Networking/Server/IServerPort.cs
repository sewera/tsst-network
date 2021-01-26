using Common.Models;
using Common.Networking.Server.Delegates;

namespace Common.Networking.Server
{
    public interface IServerPort<TPacket> where TPacket: ISerializablePacket
    {
        void Listen();
    }
}
