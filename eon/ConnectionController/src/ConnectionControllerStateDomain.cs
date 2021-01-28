using Common.Models;

namespace ConnectionController
{
    public class ConnectionControllerStateDomain : IConnectionControllerState
    {
        public ConnectionControllerStateDomain() // Add things from config
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
