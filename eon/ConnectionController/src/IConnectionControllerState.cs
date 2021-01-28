using Common.Models;

namespace ConnectionController
{
    public interface IConnectionControllerState
    {
        ResponsePacket OnConnectionRequest(RequestPacket requestPacket);
        ResponsePacket OnPeerCoordination(RequestPacket requestPacket);
    }
}
