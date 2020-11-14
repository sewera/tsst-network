using cc.Models;

namespace cc.Networking.Forwarders
{
    interface IPacketForwarder
    {
        /// <summary>Extract port serial number from incoming CcPacket
        /// and find a route for it</summary>
        /// <param name="inCcPacket">Deserialized incoming CcPacket</param>
        /// <returns>A tuple with CcConnection of where to send the packet,
        /// a packet and whether the link is alive</returns>
        (CcConnection, CcPacket, bool) ProcessPacket(CcPacket inCcPacket);
    }
}
