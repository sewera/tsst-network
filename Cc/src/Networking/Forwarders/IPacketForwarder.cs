using Cc.Models;

namespace Cc.Networking.Forwarders
{
    public interface IPacketForwarder
    {
        /// <summary>Forward packet (send it to appropriate client)</summary>
        void ForwardPacket(MplsPacket inMplsPacket);
    }
}
