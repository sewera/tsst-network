using System.Threading;
using Common.Models;
using Common.Networking.Server.Delegates;
using Common.Networking.Server.OneShot;
using Common.Startup;
using ConnectionController.Config;

namespace ConnectionController
{
    public class ConnectionControllerManager : IManager
    {
        private Configuration _configuration;

        private readonly IOneShotServerPort<RequestPacket, ResponsePacket> _connectionRequestPort;
        private readonly IOneShotServerPort<RequestPacket, ResponsePacket> _peerCoordinationPort;

        private readonly ManualResetEvent _idle = new ManualResetEvent(false);

        public ConnectionControllerManager(Configuration configuration,
                                           ReceiveRequest<RequestPacket, ResponsePacket> connectionRequestDelegate,
                                           ReceiveRequest<RequestPacket, ResponsePacket> peerCoordinationDelegate)
        {
            _configuration = configuration;
            _connectionRequestPort = new OneShotServerPort<RequestPacket, ResponsePacket>(configuration.ServerAddress,
                configuration.ConnectionRequestLocalPort);
            _peerCoordinationPort = new OneShotServerPort<RequestPacket, ResponsePacket>(configuration.ServerAddress,
                configuration.PeerCoordinationLocalPort);
            _connectionRequestPort.RegisterReceiveRequestDelegate(connectionRequestDelegate);
            _peerCoordinationPort.RegisterReceiveRequestDelegate(peerCoordinationDelegate);
        }

        public void Start()
        {
            _connectionRequestPort.Listen();
            _peerCoordinationPort.Listen();
            _idle.WaitOne();
        }
    }
}
