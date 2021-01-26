using System.Threading;
using Common.Models;
using Common.Networking.Server.OneShot;
using Common.Startup;
using NetworkCallController.Config;

namespace NetworkCallController
{
    public class NetworkCallControllerManager : IManager
    {
        private Configuration _configuration;

        private IOneShotServerPort<GenericPacket, GenericPacket> _callCoordinationPort;
        private IOneShotServerPort<GenericPacket, GenericPacket> _callTeardownPort;
        private IOneShotServerPort<GenericPacket, GenericPacket> _connectionRequestPort;

        private ManualResetEvent _idle = new ManualResetEvent(false);

        public NetworkCallControllerManager(Configuration configuration)
        {
            _configuration = configuration;
            _callCoordinationPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.CallCoordinationPort);
            _callTeardownPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.CallTeardownPort);
            _connectionRequestPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.ConnectionRequestPort);
        }

        public void Start()
        {
            _callCoordinationPort.Listen();
            _callTeardownPort.Listen();
            _connectionRequestPort.Listen();
            _idle.WaitOne();
        }
    }
}
