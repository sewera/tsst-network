using Cc.Models;
using Cc.Networking.Tables;

namespace Cc.Networking.Forwarders
{
    internal class CcPacketForwarder : IPacketForwarder
    {
        private IConnectionTable _connectionTable;

        public CcPacketForwarder(IConnectionTable connectionTable)
        {
            _connectionTable = connectionTable;
        }

        public (CcConnection, CcPacket, bool) ProcessPacket(CcPacket inCcPacket)
        {
            return (null, null, false); // TODO
        }
    }
}
