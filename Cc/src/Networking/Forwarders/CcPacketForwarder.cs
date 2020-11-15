using Cc.Models;
using Cc.Networking.Tables;

namespace Cc.Networking.Forwarders
{
    internal class CcPacketForwarder : IPacketForwarder
    {
        private IConnectionTable connectionTable;

        public CcPacketForwarder(IConnectionTable connectionTable)
        {
            this.connectionTable = connectionTable;
        }

        public (CcConnection, CcPacket, bool) ProcessPacket(CcPacket inCcPacket)
        {
            return (null, null, false); // TODO
        }
    }
}
