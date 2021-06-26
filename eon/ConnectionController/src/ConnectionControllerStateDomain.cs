using System.Collections.Generic;
using System.Linq;
using System.Net;
using Common.Api;
using Common.Models;
using Common.Utils;
using NLog;

namespace ConnectionController
{
    public class ConnectionControllerStateDomain : IConnectionControllerState
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, IApiClient<RequestPacket, ResponsePacket>> _ccConnectionRequestClients = new Dictionary<string, IApiClient<RequestPacket, ResponsePacket>>();

        private readonly Dictionary<string, string> _ccNames;

        private IPAddress _serverAddress;

        public ConnectionControllerStateDomain(IPAddress serverAddress, Dictionary<string, int> ccConnectionRequestRemotePorts, Dictionary<string, string> ccNames)
        {
            foreach ((string key, int ccConnectionRequestRemotePort) in ccConnectionRequestRemotePorts)
            {
                _ccConnectionRequestClients[key] =
                    new ApiClient<RequestPacket, ResponsePacket>(serverAddress, ccConnectionRequestRemotePort);
            }
            _ccNames = ccNames;
            _serverAddress = serverAddress;
        }

        public ResponsePacket OnConnectionRequest(RequestPacket requestPacket)
        {
            // Take ConnectionRequest params
            int id = requestPacket.Id;
            string src = requestPacket.SrcPort;
            string dst = requestPacket.DstPort;
            int sl = requestPacket.SlotsNumber;
            RequestPacket.Est est = requestPacket.Establish;

            LOG.Info($"Received CC::ConnectionRequest_req(id = {id}, src = {src}, dst = {dst}, sl = {sl}, teardown = {est})");

            // Send ConnectionRequest to domain
            LOG.Info($"Send CC::ConnectionRequest_req(id = {id}, src = {src}, dst = {dst}, sl = {sl}, teardown = {est})");
            var connectionRequestResponse = _ccConnectionRequestClients[GetCcName(src)].Get(new RequestPacket.Builder()
                .SetEst(est)
                .SetId(id)
                .SetSrcPort(src)
                .SetDstPort(dst)
                .SetSlotsNumber(sl)
                .Build());

            // Check ConnectionRequest response
            
            if (connectionRequestResponse.Res == ResponsePacket.ResponseType.ResourcesProblem) // if there were resources problem
            {
                LOG.Info("Send CC:ConnectionRequest_res(res = ResourcesProblem)");
                return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.ResourcesProblem).Build();
            }

            (int, int) slots = connectionRequestResponse.Slots;
            
            LOG.Info($"Received CC::ConnectionRequest_res({ResponsePacket.ResponseTypeToString(connectionRequestResponse.Res)}, slots = {slots}, nextZonePort = {connectionRequestResponse.NextZonePort})");

            // INTER DOMAIN CONNECTION
            if (connectionRequestResponse.NextZonePort != null)
            {
                LOG.Info("NEXT ZONE PORT != NULL");
                string nextZonePort = connectionRequestResponse.NextZonePort;
                // send Peer Coordination to second domain
                IApiClient<RequestPacket, ResponsePacket> peerCoordinationClient =  new ApiClient<RequestPacket, ResponsePacket>(_serverAddress, int.Parse("12822"));
                LOG.Info($"Send CC::PeerCoordination_req(id = {id}, src = {nextZonePort}, dst = {dst}, slots = {slots}, teardown = {est}");
                ResponsePacket peerCoordinationRes = peerCoordinationClient.Get(new RequestPacket.Builder()
                    .SetId(id)
                    .SetSrcPort(nextZonePort)
                    .SetDstPort(dst)
                    .SetSlots(slots)
                    .SetEst(est)
                    .Build()
                );
                LOG.Info($"Received CC::PeerCoordination_res(res = {peerCoordinationRes.Res})");
            }
            
            if (connectionRequestResponse.Res == ResponsePacket.ResponseType.Ok)
            {
                LOG.Info($"Send CC::ConnectionRequest_res(res = OK)");
                return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.Ok).SetSlots(slots)
                    .SetNextZonePort(connectionRequestResponse.NextZonePort)
                    .Build();
            }

            LOG.Info($"Send CC::ConnectionRequest_res(res = Refused)");
            return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.Refused).Build();
        }

        public ResponsePacket OnPeerCoordination(RequestPacket requestPacket)
        {
            LOG.Trace("EloOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            // Take PeerCoordination params
            int id = requestPacket.Id;
            string src = requestPacket.SrcPort;
            string dst = requestPacket.DstPort;
            (int, int) slots = requestPacket.Slots;
            RequestPacket.Est est = requestPacket.Establish;
            
            LOG.Info($"Received CC::PeerCoordination(id = {id}, src = {src}, dst = {dst}, slots = {slots}, teardown = {est})");
            var connectionRequestResponse = _ccConnectionRequestClients[GetCcName(src)].Get(new RequestPacket.Builder()
                .SetEst(est)
                .SetId(id)
                .SetSrcPort(src)
                .SetDstPort(dst)
                .SetSlotsNumber(slots.Item2 - slots.Item1)
                .Build());
            
            LOG.Info($"Received CC::ConnectionRequest_res({ResponsePacket.ResponseTypeToString(connectionRequestResponse.Res)}, slots = {slots}, nextZonePort = {connectionRequestResponse.NextZonePort})");
            
            if (connectionRequestResponse.Res == ResponsePacket.ResponseType.ResourcesProblem) // if there were resources problem
            {
                LOG.Info("Send CC:ConnectionRequest_res(res = ResourcesProblem)");
                return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.ResourcesProblem).Build();
            }
            if (connectionRequestResponse.Res == ResponsePacket.ResponseType.Ok)
            {
                LOG.Info($"Send CC::PeerCoordination_res(res = OK)");
                return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.Ok).SetSlots(slots)
                    .SetNextZonePort(connectionRequestResponse.NextZonePort)
                    .Build();
            }
            LOG.Info($"Send CC::PeerCoordination_res(res = Refused)");
            return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.Refused).Build();
        }

        private string GetCcName(string portAlias)
        {
            foreach (KeyValuePair<string, string> ccName in _ccNames.Where(ccName =>
                Checkers.PortMatches(ccName.Key, portAlias) > -1)) return ccName.Value; // TODO: Check for matches value
            LOG.Error($"Empty ccName from GetCcName() for portAlias: {portAlias}");
            return "";
        }
    }
}
