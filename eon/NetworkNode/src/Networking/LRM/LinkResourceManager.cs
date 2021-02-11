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

            if (lrmLinkConnectionRequestRemotePort == 0)
                _lrmConnectionRequestClient = null;
            else
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

        public void SendLocalTopologyPacketAfterWakeUp()
        {
            LOG.Info($"LRM{_localPortAlias}: Send RC::LocalTopology_req(port1 = {_localPortAlias}, port2 = {_remotePortAlias}, slotsArray = {SlotsArrayToString()})");
            ResponsePacket localTopology = _rcLocalTopologyClient.Get(new RequestPacket.Builder()
                .SetPort1(_localPortAlias)
                .SetPort2(_remotePortAlias)
                .SetSlotsArray(_slotsArray)
                .Build());
            LOG.Info($"LRM{_localPortAlias}: Received RC::LocalTopology_req(res = {ResponsePacket.ResponseTypeToString(localTopology.Res)})");
        }

        private ResponsePacket OnReceiveRequest(RequestPacket requestPacket)
        {
            // Get LRM::LinkConnectionRequest_req packet params
            GenericPacket.PacketType type = requestPacket.Type;
            (int, int) slots = requestPacket.Slots;
            RequestPacket.Who whoRequests = requestPacket.WhoRequests;
            RequestPacket.Est est = requestPacket.Establish;
            
            LOG.Info($"LRM{_localPortAlias}: Received LRM::LinkConnectionRequest_{GenericPacket.PacketTypeToString(type)}(slots = {slots}, {(est == RequestPacket.Est.Establish ? "allocate" : "deallocate")})");
            
            if (est == RequestPacket.Est.Teardown)
            {
                LOG.Debug($"Deallocating slots: {slots}");
                _slotsArray.RemoveAll(slt => slt == slots);
                return new ResponsePacket.Builder()
                    .SetEnd(_remotePortAlias)
                    .Build();
            }
            else
            {
                // Check if requested slots are free to use
                foreach ((int, int) s in _slotsArray)
                {
                    if (s == slots)
                    {
                        LOG.Debug($"LRM{_localPortAlias}: Gateway resources are already reserved.");
                        return new ResponsePacket.Builder()
                            .SetRes(ResponsePacket.ResponseType.Ok)
                            .SetEnd(_remotePortAlias)
                            .Build();
                    }

                    if (Checkers.SlotsOverlap(s, slots))
                    {
                        // If not, response with LRM::LinkConnectionRequest_res(res = REFUSED)
                        LOG.Info(
                            $"LRM{_localPortAlias}: LRM::LinkConnectionRequest_{GenericPacket.PacketTypeToString(GenericPacket.PacketType.Response)}" +
                            $"(res = {ResponsePacket.ResponseTypeToString(ResponsePacket.ResponseType.Refused)})");

                        return new ResponsePacket.Builder()
                            .SetRes(ResponsePacket.ResponseType.Refused)
                            .Build();
                    }
                }

                // Allocate
                _slotsArray.Add(slots);
            }

            // Do LocalTopology to RC server
            LOG.Info($"LRM{_localPortAlias}: Send RC::LocalTopology_req(port1 = {_localPortAlias}, port2 = {_remotePortAlias}, slotsArray = {SlotsArrayToString()})");
            ResponsePacket localTopology = _rcLocalTopologyClient.Get(new RequestPacket.Builder()
                .SetPort1(_localPortAlias)
                .SetPort2(_remotePortAlias)
                .SetSlotsArray(_slotsArray)
                .Build());

            LOG.Info($"LRM{_localPortAlias}: Received RC::LocalTopology_res(res = {ResponsePacket.ResponseTypeToString(localTopology.Res)})");
            
            // If allocation is requested by CC inform second LRM about it
            if (whoRequests == RequestPacket.Who.Cc)
            {
                if (_lrmConnectionRequestClient != null)
                {
                    LOG.Info($"LRM{_localPortAlias}: Send LRM::LinkConnectionRequest_req(slots={slots}, {(est == RequestPacket.Est.Establish ? "allocate" : "deallocate")}, who = LRM)");
                    ResponsePacket linkConnectionRequest = _lrmConnectionRequestClient.Get(new RequestPacket.Builder()
                        .SetEst(est)
                        .SetSlots(slots)
                        .SetShouldAllocate(true)
                        .SetWhoRequests(RequestPacket.Who.Lrm)
                        .Build());
                    LOG.Info(
                        $"LRM{_localPortAlias}: LRM::LinkConnectionRequest_res(res = {ResponsePacket.ResponseTypeToString(linkConnectionRequest.Res)})");
                }
            }
           
            // Send response packet
            LOG.Info($"LRM{_localPortAlias}: LRM::LinkConnectionRequest_res" +
                     $"(res = {ResponsePacket.ResponseTypeToString(ResponsePacket.ResponseType.Ok)})");
            return new ResponsePacket.Builder()
                .SetRes(ResponsePacket.ResponseType.Ok)
                .SetEnd(_remotePortAlias)
                .Build();
        }
        
        private string SlotsArrayToString()
        {
            string result = "[";
            
            foreach ((int, int) slotsTuple in _slotsArray)
            {
                result += " (" + slotsTuple.Item1 + ";" + slotsTuple.Item2 + ") ";
            }

            result += "]";
            return result;
        }
    }
}
