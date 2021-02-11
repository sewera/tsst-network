using System.Net;
using Common.Api;
using Common.Models;
using NLog;

namespace ClientNode
{
    public class CpccState
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private IApiClient<RequestPacket, ResponsePacket> _nccConnectionRequestClient;
        private IApiClient<RequestPacket, ResponsePacket> _nccCallTeardownClient;

        private int _connectionId;

        public CpccState(IPAddress nccConnectionRequestRemoteAddress,
                         int nccConnectionRequestRemotePort,
                         IPAddress nccCallTeardownRemoteAddress,
                         int nccCallTeardownRemotePort)
        {
            _nccConnectionRequestClient =
                new ApiClient<RequestPacket, ResponsePacket>(nccConnectionRequestRemoteAddress, nccConnectionRequestRemotePort);
            _nccCallTeardownClient = new ApiClient<RequestPacket, ResponsePacket>(nccCallTeardownRemoteAddress, nccCallTeardownRemotePort);
        }

        public ResponsePacket AskForConnection(string srcName, string dstName, int slotsNumber)
        {
            ResponsePacket nccConnectionRequestResponse = _nccConnectionRequestClient.Get(new RequestPacket.Builder()
                .SetSrcName(srcName)
                .SetDstName(dstName)
                .SetSlotsNumber(slotsNumber)
                .Build());

            ResponsePacket.ResponseType res = nccConnectionRequestResponse.Res;
            int connectionId = nccConnectionRequestResponse.Id;

            if (res == ResponsePacket.ResponseType.Ok)
                _connectionId = connectionId;
            return nccConnectionRequestResponse;
        }

        public ResponsePacket Teardown(int connId)
        {
            LOG.Info($"Send NCC::CallTeardown_req(connectionId = {connId})");
            return _nccCallTeardownClient.Get(new RequestPacket.Builder()
                .SetId(connId)
                .Build());
        }

        public ResponsePacket OnCallAccept(RequestPacket requestPacket)
        {
            return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.Ok).Build();
        }
    }
}
