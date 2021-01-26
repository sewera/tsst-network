using System.Threading;
using Common.Models;
using Common.Networking.Server.Delegates;
using Common.Networking.Server.OneShot;
using Common.Startup;
using NetworkCallController.Config;

namespace NetworkCallController
{
    public class NetworkCallControllerManager : IManager
    {
        private Configuration _configuration;

        private readonly IOneShotServerPort<GenericPacket, GenericPacket> _callCoordinationPort;
        private readonly IOneShotServerPort<GenericPacket, GenericPacket> _callTeardownPort;
        private readonly IOneShotServerPort<GenericPacket, GenericPacket> _connectionRequestPort;

        private readonly ManualResetEvent _idle = new ManualResetEvent(false);

        public NetworkCallControllerManager(Configuration configuration,
                                            ReceiveRequest<GenericPacket, GenericPacket> callCoordinationPortDelegate,
                                            ReceiveRequest<GenericPacket, GenericPacket> callTeardownPortDelegate,
                                            ReceiveRequest<GenericPacket, GenericPacket> connectionRequestPortDelegate)
        {
            _configuration = configuration;
            _callCoordinationPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.CallCoordinationLocalPort);
            _callTeardownPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.CallTeardownLocalPort);
            _connectionRequestPort = new OneShotServerPort<GenericPacket, GenericPacket>(configuration.ServerAddress,
                configuration.ConnectionRequestLocalPort);
            _callCoordinationPort.RegisterReceiveRequestDelegate(callCoordinationPortDelegate);
            _callTeardownPort.RegisterReceiveRequestDelegate(callTeardownPortDelegate);
            _connectionRequestPort.RegisterReceiveRequestDelegate(connectionRequestPortDelegate);
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
