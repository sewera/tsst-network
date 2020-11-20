using cc.Models;

namespace cc.Networking.Forwarders
{
    public interface IPacketForwarder
    {
        /// <summary>Forward packet (send it to appropriate client)</summary>
        void ForwardPacket(MplsPacket inMplsPacket);
    }
}
