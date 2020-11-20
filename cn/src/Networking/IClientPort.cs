using cn.Models;

namespace cn.Networking
{
    public interface IClientPort
    {
        public void Send(MplsPacket mplsPacket);
        public void ConnectToCableCloud();
        public void StartReceiving();
    }
}
