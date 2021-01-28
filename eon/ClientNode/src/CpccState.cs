using System.Net;
using Common.Api;
using Common.Models;

namespace ClientNode
{
    public class CpccState
    {
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

        public ResponsePacket AskForConnection(string srcName, string dstName, (int, int) slots)
        {
            ResponsePacket nccConnectionRequestResponse = _nccConnectionRequestClient.Get(new RequestPacket.Builder()
                .SetSrcName(srcName)
                .SetDstName(dstName)
                .SetSlots(slots)
                .Build());

            ResponsePacket.ResponseType res = nccConnectionRequestResponse.Res;
            int connectionId = nccConnectionRequestResponse.Id;

            if (res == ResponsePacket.ResponseType.Ok)
                _connectionId = connectionId;
            return nccConnectionRequestResponse;
        }

        public ResponsePacket Teardown()
        {
            return _nccCallTeardownClient.Get(new RequestPacket.Builder()
                .SetId(_connectionId)
                .Build());
        }

        public ResponsePacket OnCallAccept(RequestPacket requestPacket)
        {
            return new ResponsePacket.Builder().SetRes(ResponsePacket.ResponseType.Ok).Build();
        }
    }
}
