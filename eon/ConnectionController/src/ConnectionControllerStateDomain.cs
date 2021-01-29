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

        private readonly Dictionary<string, IApiClient<RequestPacket, ResponsePacket>> _ccPeerCoordinationClients = new Dictionary<string, IApiClient<RequestPacket, ResponsePacket>>();

        private readonly Dictionary<string, IApiClient<RequestPacket, ResponsePacket>> _ccConnectionRequestClients;
        public ConnectionControllerStateDomain(IPAddress serverAddress, Dictionary<string, int> ccPeerCoordinationRemotePorts)
        {
            foreach ((string key, int ccPeerCoordinationRemotePort) in ccPeerCoordinationRemotePorts)
            {
                _ccPeerCoordinationClients[key] =
                    new ApiClient<RequestPacket, ResponsePacket>(serverAddress, ccPeerCoordinationRemotePort);
            }
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
