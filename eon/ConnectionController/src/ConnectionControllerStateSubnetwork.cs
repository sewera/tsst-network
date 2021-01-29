using System.Collections.Generic;
using System.Net;
using Common.Api;
using Common.Models;

namespace ConnectionController
{
    public class ConnectionControllerStateSubnetwork : IConnectionControllerState
    {
        private readonly Dictionary<string, IApiClient<RequestPacket, ResponsePacket>> _ccPeerCoordinationClients = new Dictionary<string, IApiClient<RequestPacket, ResponsePacket>>();

        private readonly Dictionary<string, IApiClient<RequestPacket, ResponsePacket>> _ccConnectionRequestClients = new Dictionary<string, IApiClient<RequestPacket, ResponsePacket>>();

        private readonly Dictionary<string, IApiClient<RequestPacket, ResponsePacket>> _lrmLinkConnectionRequestClients = new Dictionary<string, IApiClient<RequestPacket, ResponsePacket>>();

        private readonly IApiClient<RequestPacket, ResponsePacket> _rcRouteTableQueryClient;

        public ConnectionControllerStateSubnetwork(IPAddress serverAddress,
                                                   Dictionary<string, int> ccPeerCoordinationRemotePorts,
                                                   Dictionary<string, int> ccConnectionRequestRemotePorts,
                                                   Dictionary<string, int> lrmLinkConnectionRequestRemotePorts,
                                                   int rcRouteTableQueryRemotePort)
        {
            foreach ((string key, int ccPeerCoordinationRemotePort) in ccPeerCoordinationRemotePorts)
            {
                _ccPeerCoordinationClients[key] =
                    new ApiClient<RequestPacket, ResponsePacket>(serverAddress, ccPeerCoordinationRemotePort);
            }
            foreach ((string key, int ccConnectionRequestRemotePort) in ccConnectionRequestRemotePorts)
            {
                _ccConnectionRequestClients[key] =
                    new ApiClient<RequestPacket, ResponsePacket>(serverAddress, ccConnectionRequestRemotePort);
            }
            foreach ((string key, int lrmLinkConnectionRequestRemotePort) in lrmLinkConnectionRequestRemotePorts)
            {
                _lrmLinkConnectionRequestClients[key] =
                    new ApiClient<RequestPacket, ResponsePacket>(serverAddress, lrmLinkConnectionRequestRemotePort);
            }

            _rcRouteTableQueryClient = new ApiClient<RequestPacket, ResponsePacket>(serverAddress, rcRouteTableQueryRemotePort);
        }

        public ResponsePacket OnConnectionRequest(RequestPacket requestPacket)
        {
            int id = requestPacket.Id;
            string src = requestPacket.SrcPort;
            string dst = requestPacket.DstPort;
            int sl = requestPacket.SlotsNumber;

            ResponsePacket routeTableQueryResponse = _rcRouteTableQueryClient.Get(new RequestPacket.Builder()
                .SetId(id)
                .SetSrcPort(src)
                .SetDstPort(dst)
                .SetSlotsNumber(sl)
                .Build());

            int rtqrId = routeTableQueryResponse.Id;
            string rtqrGateway = routeTableQueryResponse.Gateway;
            (int, int) rtqrSlots = routeTableQueryResponse.Slots;
            string dstZone = routeTableQueryResponse.DstZone;

            if (dst == rtqrGateway)
            {
                ResponsePacket connectionRequestResponseDst = _ccConnectionRequestClients[src].Get(new RequestPacket.Builder()
                    .SetId(id)
                    .SetSrcPort(src)
                    .SetDstPort(rtqrGateway)
                    .SetSlotsNumber(sl)
                    .Build());

                if (connectionRequestResponseDst.Res == ResponsePacket.ResponseType.Ok)
                    return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.Ok).SetSlots(connectionRequestResponseDst.Slots).Build();
            }

            // gateway == dstZone && dstZone != dst -- TODO Not implemented

            ResponsePacket connectionRequestResponse = _ccConnectionRequestClients[src].Get(new RequestPacket.Builder()
                .SetId(id)
                .SetSrcPort(src)
                .SetDstPort(rtqrGateway)
                .SetSlotsNumber(sl)
                .Build());

            ResponsePacket.ResponseType res = connectionRequestResponse.Res;
            (int, int) slots = connectionRequestResponse.Slots; // TODO Should slots be taken from here?

            if (res == ResponsePacket.ResponseType.Ok)
            {
                ResponsePacket linkConnectionRequestResponse = _lrmLinkConnectionRequestClients[rtqrGateway].Get(new RequestPacket.Builder()
                    .SetSlots(slots)
                    .SetShouldAllocate(true)
                    .Build());

                string end = linkConnectionRequestResponse.End;

                ResponsePacket peerCoordinationResponse = _ccPeerCoordinationClients[end].Get(new RequestPacket.Builder()
                    .SetId(id)
                    .SetEnd(end)
                    .SetDstPort(dst)
                    .SetSlots(slots)
                    .Build());

                if (peerCoordinationResponse.Res == ResponsePacket.ResponseType.Ok)
                    return new ResponsePacket.Builder()
                        .SetRes(ResponsePacket.ResponseType.Ok)
                        .SetNextZonePort("")
                        .SetSlots(slots)
                        .Build();

                // else
                return new ResponsePacket.Builder()
                    .SetRes(ResponsePacket.ResponseType.Refused)
                    .SetNextZonePort("")
                    .SetSlots(slots)
                    .Build();
            }

            // else
            return new ResponsePacket.Builder()
                .SetRes(ResponsePacket.ResponseType.Refused)
                .Build();
        }

        public ResponsePacket OnPeerCoordination(RequestPacket requestPacket)
        {
            throw new System.NotImplementedException();
        }
    }
}
