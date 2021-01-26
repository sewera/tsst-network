using System.Threading;
using Common.Models;
using Common.Networking.Server.OneShot;
using Common.Startup;
using RoutingController.Config;

namespace RoutingController
{
    public class RoutingControllerManager : IManager
    {
        private Configuration _configuration;

        private IOneShotServerPort<GenericPacket, GenericPacket> _routeTableQueryPort;
        private IOneShotServerPort<GenericPacket, GenericPacket> _localTopologyPort;
        private IOneShotServerPort<GenericPacket, GenericPacket> _networkTopologyPort;

        private ManualResetEvent _idle = new ManualResetEvent(false);

        public RoutingControllerManager(Configuration configuration)
        {
            _configuration = configuration;
            _routeTableQueryPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.RouteTableQueryLocalPort);
            _localTopologyPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.LocalTopologyLocalPort);
            _networkTopologyPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.NetworkTopologyLocalPort);
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
