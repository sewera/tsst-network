using Common.Models;

namespace ConnectionController
{
    public class ConnectionContollerStateSubnetwork : IConnectionControllerState
    {
        public ConnectionContollerStateSubnetwork()
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
