using System.Collections.Generic;
using cn.Config;
using cn.Models;
using cn.Networking;
using cn.Networking.Delegates;
using NLog;

namespace cn
{
    public class ClientNodeManager : IClientNodeManager
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        
        private Configuration _configuration;
        private IClientPortFactory _clientPortFactory;
        private IClientPort _clientPort;

        public ClientNodeManager(Configuration config, IClientPortFactory clientPortFactory)
        {
            _configuration = config;
            _clientPortFactory = clientPortFactory;
        }

        public void Start()
        {
            _clientPort = _clientPortFactory.GetPort(_configuration.ClientPortAlias);
            _clientPort.ConnectToCableCloud();
            _clientPort.StartReceiving();
        }

        public void RegisterReceiveMessageEvent(ReceiveMessage receiveMessage)
        {
            _clientPort.RegisterReceiveMessageEvent(receiveMessage);
        }

        public void Send(string destinationPortAlias, string message, List<long> labels)
        {
            MplsPacket packet = new MplsPacket.Builder()
                .SetSourcePortAlias(_configuration.ClientPortAlias)
                .SetDestinationPortAlias(destinationPortAlias)
                .SetMplsLabels(labels)
                .SetMessage(message)
                .Build();

            _clientPort.Send(packet);
        }
    }
}
