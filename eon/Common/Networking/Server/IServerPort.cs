using Common.Models;
using Common.Networking.Server.Delegates;

namespace Common.Networking.Server
{
    public interface IServerPort<TPacket> where TPacket: ISerializablePacket
    {
        void Listen();
        public void RegisterRegisterConnectionDelegate(RegisterConnection<TPacket> registerConnectionDelegate);
    }
}
