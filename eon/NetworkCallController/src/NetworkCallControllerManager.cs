using System.Collections.Generic;
using System.Threading;
using Common.Models;
using Common.Networking.Server.Delegates;
using Common.Networking.Server.OneShot;
using Common.Startup;
using NetworkCallController.Config;
using NLog;

namespace NetworkCallController
{
    public class NetworkCallControllerManager : IManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();

        private readonly Configuration _configuration;

        private readonly Dictionary<string, string> _clientPortAliases;
        private readonly Dictionary<string, string> _portDomains;
        private readonly string _domain;
        private readonly IOneShotServerPort<RequestPacket, ResponsePacket> _callCoordinationPort;
        private readonly IOneShotServerPort<RequestPacket, ResponsePacket> _callTeardownPort;
        private readonly IOneShotServerPort<RequestPacket, ResponsePacket> _connectionRequestPort;

        private readonly ManualResetEvent _idle = new ManualResetEvent(false);

        public NetworkCallControllerManager(Configuration configuration,
                                            ReceiveRequest<RequestPacket, ResponsePacket> callCoordinationPortDelegate,
                                            ReceiveRequest<RequestPacket, ResponsePacket> callTeardownPortDelegate,
                                            ReceiveRequest<RequestPacket, ResponsePacket> connectionRequestPortDelegate)
        {
            _configuration = configuration;
            _clientPortAliases = _configuration.ClientPortAliases;
            _portDomains = _configuration.PortDomains;
            _domain = _configuration.Domain;
            _callCoordinationPort = new OneShotServerPort<RequestPacket, ResponsePacket>(_configuration.ServerAddress,
                _configuration.CallCoordinationLocalPort);
            _callTeardownPort = new OneShotServerPort<RequestPacket, ResponsePacket>(_configuration.ServerAddress,
                _configuration.CallTeardownLocalPort); 
            _connectionRequestPort = new OneShotServerPort<RequestPacket, ResponsePacket>(_configuration.ServerAddress,
                _configuration.ConnectionRequestLocalPort);
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
