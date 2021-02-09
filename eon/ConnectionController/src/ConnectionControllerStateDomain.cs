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

        public ConnectionControllerStateDomain(IPAddress serverAddress, Dictionary<string, int> ccConnectionRequestRemotePorts, Dictionary<string, string> ccNames)
        {
            foreach ((string key, int ccConnectionRequestRemotePort) in ccConnectionRequestRemotePorts)
            {
                _ccConnectionRequestClients[key] =
                    new ApiClient<RequestPacket, ResponsePacket>(serverAddress, ccConnectionRequestRemotePort);
            }
            _ccNames = ccNames;
        }

        public ResponsePacket OnConnectionRequest(RequestPacket requestPacket)
        {
            int id = requestPacket.Id;
            string src = requestPacket.SrcPort;
            string dst = requestPacket.DstPort;
            int sl = requestPacket.SlotsNumber;
            LOG.Info($"Received CC::ConnectionRequest_req(id = {id}, src = {src}, dst = {dst}, sl = {sl})");

            LOG.Info($"Send CC::ConnectionRequest_req(id = {id}, src = {src}, dst = {dst}, sl = {sl})");
            var connectionRequestResponse = _ccConnectionRequestClients[GetCcName(src)].Get(new RequestPacket.Builder()
                .SetId(id)
                .SetSrcPort(src)
                .SetDstPort(dst)
                .SetSlotsNumber(sl)
                .Build());

            var res = connectionRequestResponse.Res;
            (int, int) slots = connectionRequestResponse.Slots;
            
            LOG.Info($"Received CC::ConnectionRequest_res({ResponsePacket.ResponseTypeToString(res)}, slots = {slots}, nextZonePort = NULL)");

            if (res == ResponsePacket.ResponseType.Ok)
            {
                LOG.Info($"Send CC::ConnectionRequest_res(res = OK)");
                return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.Ok).SetSlots(slots).Build();
            }

            LOG.Info($"Send CC::ConnectionRequest_res(res = Refused)");
            return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.Refused).Build();
        }

        public ResponsePacket OnPeerCoordination(RequestPacket requestPacket)
        {
            throw new System.NotImplementedException();
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
