using System.Threading;
using Common.Models;
using Common.Networking.Server.OneShot;
using Common.Startup;
using ConnectionController.Config;

namespace ConnectionController
{
    public class ConnectionControllerManager : IManager
    {
        private Configuration _configuration;

        private readonly IOneShotServerPort<GenericPacket, GenericPacket> _connectionRequestPort;
        private readonly IOneShotServerPort<GenericPacket, GenericPacket> _peerCoordinationPort;

        private readonly ManualResetEvent _idle = new ManualResetEvent(false);

        public ConnectionControllerManager(Configuration configuration)
        {
            _configuration = configuration;
            _connectionRequestPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.ConnectionRequestLocalPort);
            _peerCoordinationPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.PeerCoordinationLocalPort);
        }

        public void Start()
        {
            _connectionRequestPort.Listen();
            _peerCoordinationPort.Listen();
            _idle.WaitOne();
        }
    }
}
