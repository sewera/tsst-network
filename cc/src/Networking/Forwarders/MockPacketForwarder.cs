using cc.Models;
using cc.Networking.Tables;

namespace cc.Networking.Forwarders
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
