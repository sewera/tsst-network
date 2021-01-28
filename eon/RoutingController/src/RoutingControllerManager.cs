using System.Threading;
using Common.Models;
using Common.Networking.Server.Delegates;
using Common.Networking.Server.OneShot;
using Common.Startup;
using RoutingController.Config;

namespace RoutingController
{
    public class RoutingControllerManager : IManager
    {
        private Configuration _configuration;

        private readonly IOneShotServerPort<RequestPacket, ResponsePacket> _routeTableQueryPort;
        private readonly IOneShotServerPort<RequestPacket, ResponsePacket> _localTopologyPort;
        private readonly IOneShotServerPort<RequestPacket, ResponsePacket> _networkTopologyPort;

        private readonly ManualResetEvent _idle = new ManualResetEvent(false);

        public RoutingControllerManager(Configuration configuration,
                                        ReceiveRequest<RequestPacket, ResponsePacket> routeTableQueryDelegate,
                                        ReceiveRequest<RequestPacket, ResponsePacket> localTopologyDelegate,
                                        ReceiveRequest<RequestPacket, ResponsePacket> networkTopologyDelegate)
        {
            _configuration = configuration;
            _routeTableQueryPort = new OneShotServerPort<RequestPacket, ResponsePacket>(configuration.ServerAddress,
                configuration.RouteTableQueryLocalPort);
            _localTopologyPort = new OneShotServerPort<RequestPacket, ResponsePacket>(configuration.ServerAddress,
                configuration.LocalTopologyLocalPort);
            _networkTopologyPort = new OneShotServerPort<RequestPacket, ResponsePacket>(configuration.ServerAddress,
                configuration.NetworkTopologyLocalPort);
            _routeTableQueryPort.RegisterReceiveRequestDelegate(routeTableQueryDelegate);
            _localTopologyPort.RegisterReceiveRequestDelegate(localTopologyDelegate);
            _networkTopologyPort.RegisterReceiveRequestDelegate(networkTopologyDelegate);
        }

        public void Start()
        {
            _routeTableQueryPort.Listen();
            _localTopologyPort.Listen();
            _networkTopologyPort.Listen();
            _idle.WaitOne();
        }
    }
}
