using Common.Models;

namespace RoutingController
{
    public class RcState : IRcState
    {
        public RcState() // Add some data from config
        {}

        public ResponsePacket OnRouteTableQuery(RequestPacket requestPacket)
        {
            throw new System.NotImplementedException();
        }

        public ResponsePacket OnLocalTopology(RequestPacket requestPacket)
        {
            throw new System.NotImplementedException();
        }

        public ResponsePacket OnNetworkTopology(RequestPacket requestPacket)
        {
            throw new System.NotImplementedException();
        }
    }
}
