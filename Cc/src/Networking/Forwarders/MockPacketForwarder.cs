using Cc.Models;
using Cc.Networking.Tables;

namespace Cc.Networking.Forwarders
{
    internal class MockPacketForwarder : IPacketForwarder
    {
        private IConnectionTable _connectionTable;

        public MockPacketForwarder(IConnectionTable connectionTable)
        {
            _connectionTable = connectionTable;
        }

        public (CcConnection, CcPacket, bool) ProcessPacket(CcPacket inCcPacket)
        {
            return (null, null, false); // TODO
        }
    }
}
