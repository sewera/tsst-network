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
            _connectionTable.GetClientWorker(0).Send(inCcPacket.PortSerialNo.ToString());
            return (null, null, false);
        }
    }
}
