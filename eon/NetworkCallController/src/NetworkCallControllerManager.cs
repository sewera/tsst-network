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
                configuration.CallCoordinationLocalPort);
            _callTeardownPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.CallTeardownLocalPort);
            _connectionRequestPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.ConnectionRequestLocalPort);
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
