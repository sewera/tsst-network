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

        private readonly IOneShotServerPort<GenericDataPacket, GenericDataPacket> _callCoordinationPort;
        private readonly IOneShotServerPort<GenericDataPacket, GenericDataPacket> _callTeardownPort;
        private readonly IOneShotServerPort<GenericDataPacket, GenericDataPacket> _connectionRequestPort;

        private readonly ManualResetEvent _idle = new ManualResetEvent(false);

        public NetworkCallControllerManager(Configuration configuration,
                                            ReceiveRequest<GenericDataPacket, GenericDataPacket> callCoordinationPortDelegate,
                                            ReceiveRequest<GenericDataPacket, GenericDataPacket> callTeardownPortDelegate,
                                            ReceiveRequest<GenericDataPacket, GenericDataPacket> connectionRequestPortDelegate)
        {
            _configuration = configuration;
            _callCoordinationPort = new OneShotServerPort<GenericDataPacket, GenericDataPacket>(configuration.ServerAddress,
                configuration.CallCoordinationLocalPort);
            _callTeardownPort = new OneShotServerPort<GenericDataPacket, GenericDataPacket>(configuration.ServerAddress,
                configuration.CallTeardownLocalPort);
            _connectionRequestPort = new OneShotServerPort<GenericDataPacket, GenericDataPacket>(configuration.ServerAddress,
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
