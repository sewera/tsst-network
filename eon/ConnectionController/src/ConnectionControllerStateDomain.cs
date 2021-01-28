using System.Collections.Generic;
using System.Net;
using Common.Api;
using Common.Models;
using NLog;

namespace ConnectionController
{
    public class ConnectionControllerStateDomain : IConnectionControllerState
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private readonly IApiClient<RequestPacket, ResponsePacket> _ccPeerCoordinationClient;
        private readonly Dictionary<string, IApiClient<RequestPacket, ResponsePacket>> _ccConnectionRequestClients;
        public ConnectionControllerStateDomain(int ccConnectionRequestRemotePort, 
                                               int ccPeerCoordinationRemotePort,
                                               IPAddress serverAddress
                                               ) // Add things from config
        {
            _ccPeerCoordinationClient =
                new ApiClient<RequestPacket, ResponsePacket>(serverAddress, ccPeerCoordinationRemotePort);
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
