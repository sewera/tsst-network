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

        private readonly IOneShotServerPort<GenericPacket, GenericPacket> _routeTableQueryPort;
        private readonly IOneShotServerPort<GenericPacket, GenericPacket> _localTopologyPort;
        private readonly IOneShotServerPort<GenericPacket, GenericPacket> _networkTopologyPort;

        private readonly ManualResetEvent _idle = new ManualResetEvent(false);

        public RoutingControllerManager(Configuration configuration,
                                        ReceiveRequest<GenericPacket, GenericPacket> routeTableQueryDelegate,
                                        ReceiveRequest<GenericPacket, GenericPacket> localTopologyDelegate,
                                        ReceiveRequest<GenericPacket, GenericPacket> networkTopologyDelegate)
        {
            _configuration = configuration;
            _routeTableQueryPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.RouteTableQueryLocalPort);
            _localTopologyPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.LocalTopologyLocalPort);
            _networkTopologyPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
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
