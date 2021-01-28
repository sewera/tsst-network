using Common.Models;

namespace ConnectionController
{
    public class ConnectionControllerStateNode : IConnectionControllerState
    {
        public ConnectionControllerStateNode()
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
