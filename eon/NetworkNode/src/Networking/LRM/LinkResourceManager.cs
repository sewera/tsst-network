using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Common.Api;
using Common.Models;
using Common.Networking.Server.OneShot;
using Common.Utils;
using NLog;

namespace NetworkNode.Networking.LRM
{
    public class LinkResourceManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private IOneShotServerPort<RequestPacket, ResponsePacket> _lrmPort;
        private IApiClient<RequestPacket, ResponsePacket> _lrmConnectionRequestClient;
        private IApiClient<RequestPacket, ResponsePacket> _rcLocalTopologyClient;

        private readonly string _localPortAlias;
        private readonly string _remotePortAlias;

        private List<(int, int)> _slotsArray = new List<(int, int)>();

        public LinkResourceManager(string localPortAlias, string remotePortAlias, IPAddress serverAddress, int lrmLinkConnectionRequestLocalPort, IPAddress lrmLinkConnectionRequestRemoteAddress, int lrmLinkConnectionRequestRemotePort, IPAddress rcLocalTopologyRemoteAddress, int rcLocalTopologyRemotePort)
        {
            _localPortAlias = localPortAlias;
            _remotePortAlias = remotePortAlias;

            _lrmPort = new OneShotServerPort<RequestPacket, ResponsePacket>(serverAddress, lrmLinkConnectionRequestLocalPort);

            _lrmPort.RegisterReceiveRequestDelegate(OnReceiveRequest);

            _lrmConnectionRequestClient = new ApiClient<RequestPacket, ResponsePacket>(lrmLinkConnectionRequestRemoteAddress, lrmLinkConnectionRequestRemotePort);
            _rcLocalTopologyClient = new ApiClient<RequestPacket, ResponsePacket>(rcLocalTopologyRemoteAddress, rcLocalTopologyRemotePort);
        }

        public void Listen()
        {
            _lrmPort.Listen();
        }

        /// <summary>
        /// Only for test purposes
        /// </summary>
        /// <param name="requestPacket">Test packet</param>
        public void SendPacket(RequestPacket requestPacket)
        {
            LOG.Warn("Using test method SendPacket which should not be used manually");
            _lrmConnectionRequestClient.Get(requestPacket);
        }

        public void SendLocalTopologyPacket()
        {
            //TODO 
        }

        private ResponsePacket OnReceiveRequest(RequestPacket requestPacket)
        {
            // Get LRM::LinkConnectionRequest_req packet params
            GenericPacket.PacketType type = requestPacket.Type;
            (int, int) slots = requestPacket.Slots;
            bool shouldAllocate = requestPacket.ShouldAllocate;
            RequestPacket.Who whoRequests = requestPacket.WhoRequests;
            
            LOG.Info($"Received LRM::LinkConnectionRequest_{GenericPacket.PacketTypeToString(type)}(slots = {slots}, {(shouldAllocate ? "allocate" : "release")})");
            
            // Check if requested slots are free to use
            foreach ((int, int) s in _slotsArray)
            {
                if (Checkers.SlotsOverlap(s, slots))
                {
                    // If not, response with LRM::LinkConnectionRequest_res(res = REFUSED)
                    LOG.Info($"LRM::LinkConnectionRequest_{GenericPacket.PacketTypeToString(GenericPacket.PacketType.Response)}" +
                             $"(res = {ResponsePacket.ResponseTypeToString(ResponsePacket.ResponseType.Refused)})");
                    
                    return new ResponsePacket.Builder()
                        .SetRes(ResponsePacket.ResponseType.Refused)
                        .Build();
                }
            }
            
            // Allocate
            _slotsArray.Add(slots);
            
            // Do LocalTopology to RC server
            LOG.Info($"Send RC::LocalTopology_req(port1 = {_localPortAlias}, port2 = {_remotePortAlias}, slotsArray = {_slotsArray})");
            ResponsePacket localTopology = _rcLocalTopologyClient.Get(new RequestPacket.Builder()
                .SetPort1(_localPortAlias)
                .SetPort2(_remotePortAlias)
                .SetSlotsArray(_slotsArray)
                .Build());

            LOG.Info($"Received RC::LocalTopology_req(res = {ResponsePacket.ResponseTypeToString(localTopology.Res)}");
            
            // If allocation is requested by CC inform second LRM about it
            if (whoRequests == RequestPacket.Who.Cc)
            {
                LOG.Info($"Send LRM::LinkConnectionRequest_req(slots={slots}, allocate = true, who = LRM)");
                ResponsePacket linkConnectionRequest = _lrmConnectionRequestClient.Get(new RequestPacket.Builder()
                    .SetSlots(slots)
                    .SetShouldAllocate(true)
                    .SetWhoRequests(RequestPacket.Who.Lrm)
                    .Build());
                LOG.Info($"LRM::LinkConnectionRequest_{ResponsePacket.ResponseTypeToString(linkConnectionRequest.Res)}(res = {ResponsePacket.ResponseTypeToString(localTopology.Res)}");
            }
           
            // Send response packet
            LOG.Info($"LRM::LinkConnectionRequest_{GenericPacket.PacketTypeToString(GenericPacket.PacketType.Response)}" +
                     $"(res = {ResponsePacket.ResponseTypeToString(ResponsePacket.ResponseType.Ok)})");
            return new ResponsePacket.Builder()
                .SetRes(ResponsePacket.ResponseType.Ok)
                .SetEnd(_remotePortAlias)
                .Build();
        }
    }
}
