using Common.Models;

namespace RoutingController
{
    public interface IRcState
    {
        ResponsePacket OnRouteTableQuery(RequestPacket requestPacket);
        ResponsePacket OnLocalTopology(RequestPacket requestPacket);
        ResponsePacket OnNetworkTopology(RequestPacket requestPacket);
    }
}
