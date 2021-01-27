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

        private string _localPortAlias;
        private string _remotePortAlias;

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

        private ResponsePacket OnReceiveRequest(RequestPacket requestPacket)
        {
            (int, int) slots = requestPacket.Slots;
            bool shouldAllocate = requestPacket.ShouldAllocate;
            RequestPacket.Who whoRequests = requestPacket.WhoRequests;

            foreach ((int, int) s in _slotsArray.Where(s => Checkers.SlotsOverlap(s, slots)))
            {
                // TODO: What then?
            }

            if (whoRequests == RequestPacket.Who.Lrm)
                return new ResponsePacket.Builder()
                    .SetRes(ResponsePacket.ResponseType.Ok)
                    .SetEnd("?") // TODO: Which end?
                    .Build();

            ResponsePacket localTopology = _rcLocalTopologyClient.Get(new RequestPacket.Builder()
                .SetSrcPort(_localPortAlias)
                .SetDstPort(_remotePortAlias)
                .SetSlotsArray(_slotsArray)
                .Build());

            if (localTopology.Res == ResponsePacket.ResponseType.Ok)
            {
                LOG.Info("RC responded OK");
            }

            ResponsePacket linkConnectionRequestResponse = _lrmConnectionRequestClient.Get(new RequestPacket.Builder()
                    .SetSlots(slots)
                    .SetShouldAllocate(shouldAllocate)
                    .Build());

            return new ResponsePacket.Builder()
                .SetRes(ResponsePacket.ResponseType.Ok)
                .SetEnd("?") // TODO: Which end?
                .Build();
        }
    }
}
