using cc.Models;
using cc.Networking.Tables;

namespace cc.Networking.Forwarders
{
    class CcPacketForwarder : IPacketForwarder
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
