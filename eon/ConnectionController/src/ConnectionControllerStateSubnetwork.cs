using Common.Models;

namespace ConnectionController
{
    public class ConnectionControllerStateSubnetwork : IConnectionControllerState
    {
        public ConnectionControllerStateSubnetwork()
        {
            
        }
        public ResponsePacket OnConnectionRequest(RequestPacket requestPacket)
        {
            throw new System.NotImplementedException();
        }

        public ResponsePacket OnPeerCoordination(RequestPacket requestPacket)
        {
            throw new System.NotImplementedException();
        }
    }
}
