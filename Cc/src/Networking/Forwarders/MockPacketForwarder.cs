using Cc.Models;
using Cc.Networking.Tables;

namespace Cc.Networking.Forwarders
{
    public class MockPacketForwarder : IPacketForwarder
    {
        private readonly IConnectionTable _connectionTable;

        public MockPacketForwarder(IConnectionTable connectionTable)
        {
            _connectionTable = connectionTable;
        }

        public void ForwardPacket(MplsPacket inMplsPacket)
        {
            _connectionTable.GetCcConnection("1234").ClientWorker.Send(inMplsPacket);
        }
    }
}
