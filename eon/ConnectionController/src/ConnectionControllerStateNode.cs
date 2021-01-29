using System.Collections.Generic;
using System.Net;
using Common.Models;

namespace ConnectionController
{
    public class ConnectionControllerStateNode : IConnectionControllerState
    {
        public ConnectionControllerStateNode(IPAddress serverAddress,
                                             Dictionary<string, int> ccPeerCoordinationRemotePorts,
                                             Dictionary<string, int> ccConnectionRequestRemotePorts,
                                             int nnFibInsertRemotePort,
                                             int rcRouteTableQueryRemotePort)
        {
            throw new System.NotImplementedException();
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
