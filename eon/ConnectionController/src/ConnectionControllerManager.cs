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

        private readonly IOneShotServerPort<GenericDataPacket, GenericDataPacket> _connectionRequestPort;
        private readonly IOneShotServerPort<GenericDataPacket, GenericDataPacket> _peerCoordinationPort;

        private readonly ManualResetEvent _idle = new ManualResetEvent(false);

        public ConnectionControllerManager(Configuration configuration,
                                           ReceiveRequest<GenericDataPacket, GenericDataPacket> connectionRequestDelegate,
                                           ReceiveRequest<GenericDataPacket, GenericDataPacket> peerCoordinationDelegate)
        {
            _configuration = configuration;
            _connectionRequestPort = new OneShotServerPort<GenericDataPacket, GenericDataPacket>(configuration.ServerAddress,
                configuration.ConnectionRequestLocalPort);
            _peerCoordinationPort = new OneShotServerPort<GenericDataPacket, GenericDataPacket>(configuration.ServerAddress,
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
